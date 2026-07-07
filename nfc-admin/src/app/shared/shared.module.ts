import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NfcWriterComponent } from './components/nfc-writer/nfc-writer.component';
import { NfcDiagnosticComponent } from './components/nfc-diagnostic/nfc-diagnostic.component';

@NgModule({
  declarations: [NfcWriterComponent, NfcDiagnosticComponent],
  imports: [CommonModule],
  exports: [NfcWriterComponent, NfcDiagnosticComponent],
  // Allow Ionic web components (e.g. <ion-icon>) inside these components' templates.
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule {}
