import { NdefRecord } from '@capgo/capacitor-nfc';

export class NdefUtil {
    private static stringToBytes(str: string): number[] {
        const encoder = new TextEncoder();
        const uint8Array = encoder.encode(str);
        return Array.from(uint8Array);
    }

    /**
     * Builds an NDEF Text record byte-for-byte identical to what the
     * "NFC Tools" app writes by default:
     *   - TNF = 0x01 (TNF_WELL_KNOWN)
     *   - Type = 'T' (0x54)
     *   - Empty ID
     *   - Payload = [status byte][language code][UTF-8 text]
     *       status byte: bit 7 = 0 (UTF-8, never UTF-16) | (language length & 0x3F)
     *
     * NFC Tools defaults to the 'en' language code, so a reader that was
     * calibrated against NFC Tools sees the exact same offsets.
     */
    public static createTextRecord(
        text: string,
        languageCode: string = 'en'
    ): NdefRecord {
        // Language code is ASCII (ISO 639-1), lowercased like NFC Tools does.
        const langBytes = this.stringToBytes(languageCode.toLowerCase());
        const textBytes = this.stringToBytes(text);
        // Bit 7 (0x80) = UTF-16 flag — always 0 here because we encode UTF-8.
        const statusByte = langBytes.length & 0x3f;
        const payload = [statusByte, ...langBytes, ...textBytes];
        return {
            tnf: 0x01,
            type: [0x54],
            id: [],
            payload: payload
        };
    }

    public static createTextRecords(
        texts: string[],
        languageCode: string = 'en'
    ): NdefRecord[] {
        return texts.map(text => this.createTextRecord(text, languageCode));
    }
}
