import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { CartesPage } from './cartes.page';

const routes: Routes = [
  { path: '', component: CartesPage }
];

@NgModule({
  declarations: [CartesPage],
  imports: [CommonModule, IonicModule, RouterModule.forChild(routes)]
})
export class CartesModule {}
