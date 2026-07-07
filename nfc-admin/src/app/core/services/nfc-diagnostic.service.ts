import { Injectable } from '@angular/core';
import { CapacitorNfc, NfcEvent } from '@capgo/capacitor-nfc';
import type { PluginListenerHandle } from '@capacitor/core';
import { NgZone } from '@angular/core';

export interface TagDiagnostic {
  type: string | null;
  techTypes: string[];
  maxSize: number | null;
  isWritable: boolean | null;
  canMakeReadOnly: boolean | null;
  ndefMessageLength: number;
  records: Array<{
    tnf: number;
    typeHex: string;
    payloadHex: string;
    payloadLength: number;
  }>;
  rawIdHex: string;
}

@Injectable({ providedIn: 'root' })
export class NfcDiagnosticService {
  private listener: PluginListenerHandle | null = null;

  constructor(private zone: NgZone) {}

  async scanTag(timeoutMs = 10000): Promise<TagDiagnostic> {
    return new Promise((resolve, reject) => {
      let settled = false;

      const cleanup = async () => {
        if (this.listener) {
          try { await this.listener.remove(); } catch {}
          this.listener = null;
        }
        try { await CapacitorNfc.stopScanning(); } catch {}
      };

      const timer = setTimeout(() => {
        if (!settled) {
          settled = true;
          cleanup();
          reject(new Error('Délai de scan écoulé (timeout).'));
        }
      }, timeoutMs);

      this.zone.runOutsideAngular(async () => {
        try {
          await CapacitorNfc.addListener('nfcEvent', (event: NfcEvent) => {
            if (settled) return;
            settled = true;
            clearTimeout(timer);
            cleanup();

            const tag = event.tag;
            const records: TagDiagnostic['records'] = [];

            if (tag.ndefMessage && Array.isArray(tag.ndefMessage)) {
              for (const r of tag.ndefMessage) {
                records.push({
                  tnf: r.tnf ?? 0,
                  typeHex: (r.type || []).map(b => (b & 0xFF).toString(16).padStart(2, '0')).join(' '),
                  payloadHex: (r.payload || []).map(b => (b & 0xFF).toString(16).padStart(2, '0')).join(' '),
                  payloadLength: (r.payload || []).length
                });
              }
            }

            resolve({
              type: tag.type ?? null,
              techTypes: tag.techTypes ?? [],
              maxSize: tag.maxSize ?? null,
              isWritable: tag.isWritable ?? null,
              canMakeReadOnly: tag.canMakeReadOnly ?? null,
              ndefMessageLength: tag.ndefMessage ? tag.ndefMessage.length : 0,
              records,
              rawIdHex: tag.id ? this.bytesToHex(tag.id) : ''
            });
          });
        } catch (err: any) {
          if (!settled) {
            settled = true;
            clearTimeout(timer);
            cleanup();
            reject(err);
          }
        }
      });
    });
  }

  private bytesToHex(bytes: number[]): string {
    return bytes.map(b => (b & 0xFF).toString(16).padStart(2, '0')).join(' ').toUpperCase();
  }
}
