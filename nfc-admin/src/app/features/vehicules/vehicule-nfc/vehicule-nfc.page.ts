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
  selector: 'app-vehicule-nfc',
  templateUrl: './vehicule-nfc.page.html'
})
export class VehiculeNfcPage implements OnInit, OnDestroy {
  nfcData: NfcData | null = null;
  nfcSupported = false;
  status: 'idle' | 'scanning' | 'writing' | 'success' | 'error' = 'idle';
  errorMsg = '';
  private nfcListener: PluginListenerHandle | null = null;

  constructor(private router: Router, private ngZone: NgZone) {}

  async ngOnInit(): Promise<void> {
    const state = history.state;
    this.nfcData = state?.nfcData ?? null;
    await this.checkNfcSupport();
    if (!this.nfcData) this.router.navigate(['/vehicules']);
  }

  ngOnDestroy(): void {
    this.removeListener();
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
    console.log('🚀 VehiculeNfcPage startWrite called');
    if (!this.nfcData) return;

    if (!this.nfcSupported) {
      this.status = 'error';
      this.errorMsg = 'NFC non disponible sur cet appareil.';
      return;
    }

    this.status = 'scanning';

    try {
      await this.removeListener();
      console.log('📻 VehiculeNfcPage adding nfcEvent listener');
      this.nfcListener = await CapacitorNfc.addListener('nfcEvent', (event: NfcEvent) => {
        console.log('📡 VehiculeNfcPage nfcEvent received:', event);
        this.ngZone.run(() => this.handleTagDetected(event));
      });

      console.log('🔍 VehiculeNfcPage starting NFC scan');
      await CapacitorNfc.startScanning({
        alertMessage: 'Approchez la carte NFC',
        invalidateAfterFirstRead: false,
        androidReaderModeFlags: ANDROID_READER_FLAGS_WITH_NDEF
      });
      console.log('✅ VehiculeNfcPage NFC scan started');
    } catch (err: any) {
      console.error('❌ VehiculeNfcPage error starting scan:', err);
      this.status = 'error';
      this.errorMsg = err?.message ?? 'Erreur lors du démarrage du scan NFC.';
      this.removeListener();
    }
  }

  private async handleTagDetected(event: NfcEvent): Promise<void> {
    try {
      const texts = [this.nfcData!.record1];
      if (this.nfcData?.record2 != null) {
        texts.push(this.nfcData.record2);
      }
      const records = NdefUtil.createTextRecords(texts, 'en');

      this.status = 'writing';
      // Single write attempt, exactly like NFC Tools: allowFormat lets the
      // plugin NDEF-format blank/formatable tags (NTAG, Ultralight, MIFARE
      // Classic with default keys) before writing. Retrying a second write on
      // the same tap consumes the session and throws "Tag became stale".
      await CapacitorNfc.write({
        records,
        allowFormat: true,
      });
      await this.cleanupAfterWrite();
      this.status = 'success';
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
      this.cleanupAfterWrite();
    }
  }

  private async cleanupAfterWrite(): Promise<void> {
    try {
      await CapacitorNfc.stopScanning();
    } catch (stopErr) {
      console.warn('⚠️ VehiculeNfcPage error stopping scan:', stopErr);
    }
    try {
      await this.removeListener();
    } catch (listenerErr) {
      console.warn('⚠️ VehiculeNfcPage error removing listener:', listenerErr);
    }
  }

  cancelWrite(): void {
    this.status = 'idle';
  }

  goBack(): void {
    this.removeListener();
    this.router.navigate(['/vehicules']);
  }

  private async removeListener(): Promise<void> {
    if (this.nfcListener) {
      await this.nfcListener.remove();
      this.nfcListener = null;
    }
  }
}