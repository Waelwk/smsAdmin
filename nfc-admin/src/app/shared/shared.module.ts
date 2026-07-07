import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NfcWriterComponent } from './components/nfc-writer/nfc-writer.component';

@NgModule({
  declarations: [NfcWriterComponent],
  imports: [CommonModule],
  exports: [NfcWriterComponent],
  // Allow Ionic web components (e.g. <ion-icon>) inside these components' templates.
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule {}
