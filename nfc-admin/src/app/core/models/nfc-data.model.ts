/** Données NFC retournées par l'API à écrire sur la carte physique */
export interface NfcData {
  record1: string;
  record2: string | null;
}
