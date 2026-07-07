import { Component } from '@angular/core';
import { NfcDiagnosticService, TagDiagnostic } from '../../../core/services/nfc-diagnostic.service';

@Component({
  selector: 'app-nfc-diagnostic',
  template: `
    <div class="card">
      <div class="card-header"><span class="card-title">Diagnostic carte NFC</span></div>
      <div class="card-body">
        <button class="btn btn-outline btn-sm" (click)="runDiagnostic()">
          <ion-icon name="scan-outline"></ion-icon> Scanner la carte
        </button>
        <div style="margin-top:8px;" *ngIf="diagnosticError" style="color:var(--c-danger);">{{ diagnosticError }}</div>
        <div style="margin-top:8px;" *ngIf="diagnostic">
          <div style="font-size:12px;"><strong>Type :</strong> {{ diagnostic.type || 'inconnu' }}</div>
          <div style="font-size:12px;"><strong>Technologies :</strong> {{ diagnostic.techTypes.join(', ') || 'aucune' }}</div>
          <div style="font-size:12px;"><strong>Taille max :</strong> {{ diagnostic.maxSize }} octets</div>
          <div style="font-size:12px;"><strong>Lecture seule :</strong> {{ diagnostic.isWritable === false ? 'oui' : 'non' }}</div>
          <div style="font-size:12px;"><strong>NDEF prêt :</strong> {{ diagnostic.ndefMessageLength > 0 ? 'oui' : 'non' }}</div>
          <div style="font-size:12px;"><strong>Records NDEF :</strong> {{ diagnostic.ndefMessageLength }}</div>
          <div *ngFor="let rec of diagnostic.records; let i = index" style="margin-top:6px; padding:6px; background:var(--c-bg); border-radius:4px; font-size:11px;">
            <div><strong>Record {{ i + 1 }}</strong></div>
            <div>TNF: {{ rec.tnf }} (0x{{ rec.tnf.toString(16) }})</div>
            <div>Type: {{ rec.typeHex }}</div>
            <div>Payload: {{ rec.payloadHex || 'vide' }}</div>
            <div>Payload length: {{ rec.payloadLength }} octets</div>
          </div>
          <div style="font-size:12px; margin-top:6px;"><strong>UID (hex) :</strong> {{ diagnostic.rawIdHex || 'inconnu' }}</div>
        </div>
      </div>
    </div>
  `
})
export class NfcDiagnosticComponent {
  diagnostic: TagDiagnostic | null = null;
  diagnosticError = '';

  constructor(private diagnosticService: NfcDiagnosticService) {}

  async runDiagnostic(): Promise<void> {
    this.diagnostic = null;
    this.diagnosticError = '';
    try {
      this.diagnostic = await this.diagnosticService.scanTag();
    } catch (err: any) {
      this.diagnosticError = err?.message ?? 'Erreur inconnue';
    }
  }
}
