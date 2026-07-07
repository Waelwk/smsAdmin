import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { ResponsableNfcPage } from './responsable-nfc.page';

const routes: Routes = [
  { path: '', component: ResponsableNfcPage }
];

@NgModule({
  declarations: [ResponsableNfcPage],
  imports: [CommonModule, FormsModule, IonicModule, RouterModule.forChild(routes)],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ResponsableNfcModule {}
