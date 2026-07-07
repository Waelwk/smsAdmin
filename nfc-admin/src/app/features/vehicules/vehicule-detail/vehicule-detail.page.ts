import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehiculeService } from '../../../core/services/vehicule.service';
import { CarteService } from '../../../core/services/carte.service';
import { VehiculeDetail } from '../../../core/models/vehicule.model';

@Component({
  selector: 'app-vehicule-detail',
  templateUrl: './vehicule-detail.page.html'
})
export class VehiculeDetailPage implements OnInit {
  vehicule: VehiculeDetail | null = null;
  loading = false;
  ctagManuel = '';
  showCtagInput = false;
  message = '';
  messageType: 'success' | 'error' = 'success';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehiculeService: VehiculeService,
    private carteService: CarteService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.load(id);
  }

  private load(id: string): void {
    this.loading = true;
    this.vehiculeService.getById(id).subscribe({
      next: res => { this.vehicule = res.data; this.loading = false; },
      error: () => this.loading = false
    });
  }

  generateCTag(): void {
    if (!this.vehicule) return;
    this.vehiculeService.generateCTag(this.vehicule.cVehicule).subscribe({
      next: () => this.load(this.vehicule!.cVehicule),
      error: err => { this.message = err.message; this.messageType = 'error'; }
    });
  }

  saveCTagManuel(): void {
    if (!this.vehicule || !this.ctagManuel.trim()) return;
    this.vehiculeService.updateCTag(this.vehicule.cVehicule, this.ctagManuel.trim()).subscribe({
      next: () => {
        this.showCtagInput = false;
        this.ctagManuel = '';
        this.load(this.vehicule!.cVehicule);
      },
      error: err => { this.message = err.message; this.messageType = 'error'; }
    });
  }

  creerCarte(): void {
    if (!this.vehicule) return;
    this.carteService.getCarteVehicule(this.vehicule.cVehicule).subscribe({
      next: res => this.router.navigate(
        ['/vehicules', this.vehicule!.cVehicule, 'nfc'],
        { state: { nfcData: res.data } }
      ),
      error: err => { this.message = err.message; this.messageType = 'error'; }
    });
  }
}
