import { Component } from '@angular/core';

@Component({
  selector: 'app-cartes',
  template: `
    <div class="pg">
      <div class="pg-header">
        <h1 class="pg-title">Cartes NFC</h1>
        <p class="pg-sub">Gestion des cartes NFC chauffeurs et véhicules</p>
      </div>
      <div class="empty-state">
        <div class="empty-icon"><ion-icon name="card-outline"></ion-icon></div>
        <div class="empty-title">Sélectionnez un chauffeur ou un véhicule</div>
        <div class="empty-desc">Accédez aux détails d'un chauffeur ou véhicule pour créer sa carte NFC.</div>
        <div style="display:flex;gap:12px;margin-top:20px;flex-wrap:wrap;justify-content:center;">
          <button class="btn btn-primary" routerLink="/chauffeurs">👤 Chauffeurs</button>
          <button class="btn btn-outline" routerLink="/vehicules">🚗 Véhicules</button>
        </div>
      </div>
    </div>
  `
})
export class CartesPage {}
