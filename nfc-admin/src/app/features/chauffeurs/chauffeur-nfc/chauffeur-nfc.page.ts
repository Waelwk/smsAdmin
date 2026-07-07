import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { NfcData } from '../../../core/models/nfc-data.model';
import { CapacitorNfc, NfcEvent } from '@capgo/capacitor-nfc';
import type { PluginListenerHandle } from '@capacitor/core';
import { NdefUtil } from '../../../core/services/ndef.util';

/**
 * Android reader-mode flags WITHOUT FLAG_READER_SKIP_NDEF_CHECK (0x80).
 *
 * The plugin's default flags skip the NDEF check, which makes Android return
 * null for both Ndef.get(tag) and NdefFormatable.get(tag). For MIFARE Classic
 * that forces the plugin into a broken raw-block write that NFC Tools cannot
 * read. By keeping the NDEF check enabled the plugin uses the platform's
 * NdefFormatable.format() — the correct MIFARE Classic NDEF formatter (MAD +
 * TLV + NFC Forum keys), matching NFC Tools' behaviour.
 *
 *   NFC_A(0x1) | NFC_B(0x2) | NFC_F(0x4) | NFC_V(0x8) | NO_PLATFORM_SOUNDS(0x100)
 */
const ANDROID_READER_FLAGS_WITH_NDEF = 0x1 | 0x2 | 0x4 | 0x8 | 0x100; // 271

@Component({
  selector: 'app-chauffeur-nfc',
  templateUrl: './chauffeur-nfc.page.html'
})
export class ChauffeurNfcPage implements OnInit, OnDestroy {
  nfcData: NfcData | null = null;
  nfcSupported = false;
  status: 'idle' | 'scanning' | 'writing' | 'success' | 'error' = 'idle';
  errorMsg = '';
  showPassword = false;
  currentStep = 0;
  steps = ['Données chargées', 'Approcher la carte', 'Écriture terminée'];
  private nfcListener: PluginListenerHandle | null = null;
  private scanning = false;

  constructor(private router: Router, private ngZone: NgZone) {}

  async ngOnInit(): Promise<void> {
    const state = history.state;
    this.nfcData = state?.nfcData ?? null;
    await this.checkNfcSupport();
    if (!this.nfcData) this.router.navigate(['/chauffeurs']);
  }

  // Ionic lifecycle: release the NFC session when leaving the page so Android's
  // default NFC reader can be used by other apps again.
  ionViewWillLeave(): void {
    this.disarmScanner();
  }

  ngOnDestroy(): void {
    this.disarmScanner();
  }

  private async checkNfcSupport(): Promise<void> {
    try {
      const result = await CapacitorNfc.isSupported();
      this.nfcSupported = result.supported;
    } catch {
      this.nfcSupported = false;
    }
  }

  async startWrite(): Promise<void> {
    if (!this.nfcData) return;

    if (!this.nfcSupported) {
      this.status = 'error';
      this.errorMsg = 'NFC non disponible sur cet appareil.';
      return;
    }

    this.status = 'scanning';
    this.currentStep = 1;
    await this.armScanner();
  }

  /** Start (or keep) the NFC scanning session + listener. Idempotent. */
  private async armScanner(): Promise<void> {
    if (this.scanning || !this.nfcSupported) return;
    try {
      if (!this.nfcListener) {
        this.nfcListener = await CapacitorNfc.addListener('nfcEvent', (event: NfcEvent) => {
          this.ngZone.run(() => this.handleTagDetected(event));
        });
      }
      await CapacitorNfc.startScanning({
        alertMessage: 'Approchez la carte NFC',
        invalidateAfterFirstRead: false,
        androidReaderModeFlags: ANDROID_READER_FLAGS_WITH_NDEF
      });
      this.scanning = true;
    } catch (err: any) {
      console.error('Error arming NFC scanner:', err);
      this.status = 'error';
      this.errorMsg = err?.message ?? 'Erreur lors du démarrage du scan NFC.';
    }
  }

  /**
   * Re-arm the reader mode after a write so the listener stays active and the
   * next tag is handled by this app. stopScanning()+startScanning() refreshes
   * the foreground (reader mode) session on both Android & iOS.
   */
  private async rearmScanner(): Promise<void> {
    this.scanning = false;
    try { await CapacitorNfc.stopScanning(); } catch { /* already stopped */ }
    try {
      await CapacitorNfc.startScanning({
        alertMessage: 'Approchez la carte NFC',
        invalidateAfterFirstRead: false,
        androidReaderModeFlags: ANDROID_READER_FLAGS_WITH_NDEF
      });
      this.scanning = true;
    } catch (err: any) {
      console.error('Error re-arming NFC scanner:', err);
    }
  }

  private async handleTagDetected(event: NfcEvent): Promise<void> {
    if (this.status === 'writing') return; // ignore concurrent events
    if (!this.nfcData) return;

    try {
      const texts = [this.nfcData.record1];
      if (this.nfcData?.record2 != null) {
        texts.push(this.nfcData.record2);
      }
      const records = NdefUtil.createTextRecords(texts, 'en');

      this.status = 'writing';
      // Single write attempt, exactly like NFC Tools: allowFormat lets the
      // plugin NDEF-format blank/formatable tags (NTAG, Ultralight, MIFARE
      // Classic with default keys) before writing.
      await CapacitorNfc.write({
        records,
        allowFormat: true,
      });

      this.status = 'success';
      this.currentStep = 2;
      // Keep the app in the foreground: re-arm so the next tag is handled by
      // this app and Android's default NFC reader never launches.
      await this.rearmScanner();
    } catch (err: any) {
      this.status = 'error';
      const msg = err?.message ?? "Erreur lors de l'écriture NFC.";
      if (msg.includes('MifareClassic authentication failed')) {
        this.errorMsg = "Cette carte MIFARE Classic utilise des clés personnalisées ou est verrouillée. Utilisez des cartes vierges MIFARE Ultralight / NTAG (formatables NDEF) ou des cartes MIFARE Classic avec clés par défaut.";
      } else if (msg.includes('Tag became stale')) {
        this.errorMsg = "La carte a été retirée trop vite. Approchez-la à nouveau et réessayez.";
      } else {
        this.errorMsg = msg;
      }
      // Keep listening so the user can simply tap the card again to retry.
      await this.rearmScanner();
    }
  }

  /** Stop the scanning session + remove the listener (page left / cancelled). */
  private async disarmScanner(): Promise<void> {
    this.scanning = false;
    try { await CapacitorNfc.stopScanning(); } catch { /* ignore */ }
    if (this.nfcListener) {
      try { await this.nfcListener.remove(); } catch { /* ignore */ }
      this.nfcListener = null;
    }
  }

  cancelWrite(): void {
    this.disarmScanner();
    this.status = 'idle';
    this.currentStep = 0;
  }

  reset(): void {
    this.status = 'idle';
    this.currentStep = 0;
    this.armScanner();
  }

  goBack(): void {
    this.disarmScanner();
    this.router.navigate(['/chauffeurs']);
  }
}
