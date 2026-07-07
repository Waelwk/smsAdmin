import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { IonicModule } from '@ionic/angular';

import { ChauffeurListPage, WithCTagPipe, WithPasswordPipe, NoCTagPipe } from './chauffeur-list/chauffeur-list.page';
import { ChauffeurDetailPage } from './chauffeur-detail/chauffeur-detail.page';
import { ChauffeurNfcPage } from './chauffeur-nfc/chauffeur-nfc.page';

const routes: Routes = [
  { path: '',              component: ChauffeurListPage },
  { path: ':matricule',    component: ChauffeurDetailPage },
  { path: ':matricule/nfc', component: ChauffeurNfcPage }
];

import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    ChauffeurListPage, ChauffeurDetailPage, ChauffeurNfcPage,
    WithCTagPipe, WithPasswordPipe, NoCTagPipe
  ],
  imports: [CommonModule, FormsModule, IonicModule, RouterModule.forChild(routes), SharedModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ChauffeursModule {}
