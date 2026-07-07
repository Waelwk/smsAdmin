import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { IonicModule } from '@ionic/angular';

import { VehiculeListPage, WithVCTagPipe, NoVCTagPipe } from './vehicule-list/vehicule-list.page';
import { VehiculeDetailPage } from './vehicule-detail/vehicule-detail.page';
import { VehiculeNfcPage } from './vehicule-nfc/vehicule-nfc.page';

const routes: Routes = [
  { path: '',      component: VehiculeListPage },
  { path: ':id',   component: VehiculeDetailPage },
  { path: ':id/nfc', component: VehiculeNfcPage }
];

import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    VehiculeListPage, VehiculeDetailPage, VehiculeNfcPage,
    WithVCTagPipe, NoVCTagPipe
  ],
  imports: [CommonModule, FormsModule, IonicModule, RouterModule.forChild(routes), SharedModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class VehiculesModule {}
