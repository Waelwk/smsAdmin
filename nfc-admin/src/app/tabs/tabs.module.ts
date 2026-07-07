import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { TabsComponent } from './tabs.component';

const routes: Routes = [
  {
    path: '',
    component: TabsComponent,
    children: [
      {
        path: 'dashboard',
        loadChildren: () => import('../features/dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'chauffeurs',
        loadChildren: () => import('../features/chauffeurs/chauffeurs.module').then(m => m.ChauffeursModule)
      },
      {
        path: 'vehicules',
        loadChildren: () => import('../features/vehicules/vehicules.module').then(m => m.VehiculesModule)
      },
      {
        path: 'cartes',
        loadChildren: () => import('../features/cartes/cartes.module').then(m => m.CartesModule)
      },
      {
        path: 'parametres',
        loadChildren: () => import('../features/parametres/parametres.module').then(m => m.ParametresModule)
      },
      {
        path: 'creation-carte-responsable',
        loadChildren: () => import('../features/responsable-nfc/responsable-nfc.module').then(m => m.ResponsableNfcModule)
      },
      { path: '', redirectTo: 'chauffeurs', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  declarations: [TabsComponent],
  imports: [CommonModule, IonicModule, RouterModule.forChild(routes)],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TabsModule {}
