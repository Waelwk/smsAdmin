import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'fr.nfcadmin',
  appName: 'NFC Admin',
  webDir: 'dist/nfc-admin',
  server: {
    androidScheme: 'http',
    cleartext: true
  }
};

export default config;