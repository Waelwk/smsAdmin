import { Component, Input, OnDestroy } from '@angular/core';
import { NfcData } from '../../../core/models/nfc-data.model';

@Component({
  selector: 'app-nfc-writer',
  templateUrl: './nfc-writer.component.html'
})
export class NfcWriterComponent implements OnDestroy {
  @Input() nfcData!: NfcData;

  status: 'idle' | 'waiting' | 'success' | 'error' = 'idle';
  statusMessage = '';
  private abortController: AbortController | null = null;

  get nfcSupported(): boolean { return 'NDEFReader' in window; }

  async write(): Promise<void> {
    if (!this.nfcSupported) {
      this.status = 'error';
      this.statusMessage = 'NFC non disponible sur cet appareil.';
      return;
    }
    if (!this.nfcData?.record1) {
      this.status = 'error';
      this.statusMessage = 'Données NFC manquantes.';
      return;
    }
    this.status = 'waiting';
    this.statusMessage = 'Approchez la carte NFC...';
    this.abortController = new AbortController();
    try {
      const ndef = new (window as any).NDEFReader();
      const records: any[] = [{ recordType: 'text', data: this.nfcData.record1 }];
      if (this.nfcData.record2) records.push({ recordType: 'text', data: this.nfcData.record2 });
      await ndef.write({ records }, { signal: this.abortController.signal });
      this.status = 'success';
      this.statusMessage = 'Carte NFC écrite avec succès.';
    } catch (err: any) {
      if (err?.name === 'AbortError') { this.status = 'idle'; this.statusMessage = ''; }
      else { this.status = 'error'; this.statusMessage = err?.message ?? 'Erreur inconnue'; }
    }
  }

  cancel(): void { this.abortController?.abort(); this.status = 'idle'; this.statusMessage = ''; }
  ngOnDestroy(): void { this.abortController?.abort(); }
}
