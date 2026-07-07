import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { CapacitorNfc, NfcEvent } from '@capgo/capacitor-nfc';
import type { PluginListenerHandle } from '@capacitor/core';
import { NdefUtil } from '../../core/services/ndef.util';

/**
 * Android reader-mode flags (NfcAdapter.enableReaderMode).
 *
 *   NFC_A(0x1) | NFC_B(0x2) | NFC_F(0x4) | NFC_V(0x8) | NO_PLATFORM_SOUNDS(0x100)
 *
 * Reader mode gives this app foreground priority over Android's default NFC
 * dispatch, so the system NFC reader / another NFC app never launches while
 * the page is visible. NDEF checks are kept enabled (no 0x80 flag) so the
 * plugin formats MIFARE Classic correctly, matching NFC Tools' behaviour.
 */
const ANDROID_READER_MODE_FLAGS = 0x1 | 0x2 | 0x4 | 0x8 | 0x100;

@Component({
  selector: 'app-responsable-nfc',
  templateUrl: './responsable-nfc.page.html'
})
export class ResponsableNfcPage implements OnInit, OnDestroy {
  codeClient = '';
  codeResponsable = '';
  nfcSupported = false;
  status: 'idle' | 'scanning' | 'writing' | 'success' | 'error' = 'idle';
  errorMsg = '';

  private nfcListener: PluginListenerHandle | null = null;
  private scanning = false;

  constructor(private router: Router, private ngZone: NgZone) {}

  ngOnInit(): void {
    this.checkNfcSupport();
  }

  /**
   * Ionic lifecycle: the page became the active view.
   * Arm the NFC listener immediately so the app keeps foreground priority and
   * Android's default NFC reader can never open while this page is visible.
   */
  ionViewDidEnter(): void {
    this.armScanner();
  }

  /**
   * Ionic lifecycle: the page is about to be left.
   * Release the NFC session so other apps (and Android's default) can use NFC
   * again. This is the ONLY place scanning is stopped — never after a write.
   */
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

  onCodeClientInput(event: any): void {
    this.codeClient = String(event.target.value ?? '').replace(/\s/g, '').replace(/[^0-9]/g, '');
    this.codeResponsable = this.codeClient ? this.codeClient + 'RE' : '';
  }

  generateCode(): void {
    const cleaned = this.codeClient.replace(/\s/g, '').replace(/[^0-9]/g, '');
    if (!cleaned) {
      this.status = 'error';
      this.errorMsg = 'Le code client est obligatoire.';
      return;
    }
    this.codeClient = cleaned;
    this.codeResponsable = cleaned + 'RE';
    this.status = 'idle';
    this.errorMsg = '';
    this.armScanner();
  }

  /**
   * "Écrire sur la carte NFC" : ensure a code exists, then make sure the
   * scanner is armed and show the listening state. The actual write happens
   * automatically when a tag is detected (see handleTagDetected).
   */
  startWrite(): void {
    if (!this.codeResponsable) {
      this.status = 'error';
      this.errorMsg = 'Le code client est obligatoire.';
      return;
    }
    if (!this.nfcSupported) {
      this.status = 'error';
      this.errorMsg = 'NFC non disponible sur cet appareil.';
      return;
    }
    this.status = 'scanning';
    this.armScanner();
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
        androidReaderModeFlags: ANDROID_READER_MODE_FLAGS
      });
      this.scanning = true;
    } catch (err: any) {
      console.error('Error arming NFC scanner:', err);
      this.status = 'error';
      this.errorMsg = err?.message ?? "Erreur lors de l'activation du scan NFC.";
    }
  }

  /**
   * Re-arm the reader mode after a write so the listener stays active and the
   * next tag is handled by this app. stopScanning()+startScanning() guarantees
   * the foreground (reader mode) session is refreshed on both Android & iOS.
   */
  private async rearmScanner(): Promise<void> {
    this.scanning = false;
    try { await CapacitorNfc.stopScanning(); } catch { /* already stopped */ }
    try {
      await CapacitorNfc.startScanning({
        alertMessage: 'Approchez la carte NFC',
        invalidateAfterFirstRead: false,
        androidReaderModeFlags: ANDROID_READER_MODE_FLAGS
      });
      this.scanning = true;
    } catch (err: any) {
      console.error('Error re-arming NFC scanner:', err);
    }
  }

  private async handleTagDetected(event: NfcEvent): Promise<void> {
    // Guard against concurrent events for the same tag / re-entrancy.
    if (this.status === 'writing') return;
    // Nothing to write yet (no code generated) → just keep listening.
    if (!this.codeResponsable) return;

    try {
      const records = NdefUtil.createTextRecords([this.codeResponsable], 'en');
      this.status = 'writing';
      await CapacitorNfc.write({
        records,
        allowFormat: true,
      });
      this.status = 'success';
      // Keep the app in the foreground: re-arm so the next tag is handled by
      // this app and Android's default NFC reader never launches.
      await this.rearmScanner();
    } catch (err: any) {
      const msg = err?.message ?? "Erreur lors de l'écriture NFC.";
      this.status = 'error';
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

  /** Stop the scanning session + remove the listener (page left). */
  private async disarmScanner(): Promise<void> {
    this.scanning = false;
    try { await CapacitorNfc.stopScanning(); } catch { /* ignore */ }
    if (this.nfcListener) {
      try { await this.nfcListener.remove(); } catch { /* ignore */ }
      this.nfcListener = null;
    }
  }

  reset(): void {
    this.status = 'idle';
    this.errorMsg = '';
    this.armScanner();
  }

  goBack(): void {
    this.disarmScanner();
    this.router.navigate(['/parametres']);
  }
}
