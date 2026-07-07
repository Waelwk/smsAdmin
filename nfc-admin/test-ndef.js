
// Simulate what phonegap-nfc's ndef.textRecord does
// Based on don/ndef-js source

function stringToBytes(str) {
    const encoder = new TextEncoder();
    return Array.from(encoder.encode(str));
}

function encodeTextPayload(text, lang = 'en') {
    // ISO/IANA language code, but we're not enforcing
    if (!lang) { lang = 'en'; }
    const encoded = stringToBytes(lang + text);
    encoded.unshift(lang.length);
    return encoded;
}

function createTextRecord(text, lang = 'fr') {
    return {
        tnf: 0x01, // TNF_WELL_KNOWN
        type: [0x54], // RTD_TEXT
        id: [],
        payload: encodeTextPayload(text, lang)
    };
}

console.log("Testing text record for 'test123' with lang 'fr':");
const record = createTextRecord('test123', 'fr');
console.log('Record:', record);
console.log('Payload as bytes:', record.payload);
console.log('Payload as hex:', record.payload.map(b => b.toString(16).padStart(2, '0')).join(' '));
console.log('Payload decoded (raw):', new TextDecoder().decode(new Uint8Array(record.payload)));
