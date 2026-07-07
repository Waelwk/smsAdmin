import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { ParametresPage } from './parametres.page';

const routes: Routes = [
  { path: '', component: ParametresPage }
];

@NgModule({
  declarations: [ParametresPage],
  imports: [CommonModule, IonicModule, RouterModule.forChild(routes)]
})
export class ParametresModule {}
