import { Component, OnInit } from '@angular/core';
import { EmployeService } from '../../core/services/employe.service';
import { VehiculeService } from '../../core/services/vehicule.service';
import { BehaviorSubject } from 'rxjs';
import { forkJoin } from 'rxjs';

interface Stat {
  label: string;
  value: string;
  icon: string;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.page.html'
})
export class DashboardPage implements OnInit {
  stats$ = new BehaviorSubject<Stat[]>([
    { label: 'Total Chauffeurs', value: '—', icon: 'people-outline',  color: 'night'   },
    { label: 'Cartes NFC actives', value: '—', icon: 'card-outline',  color: 'sky'     },
    { label: 'Véhicules actifs', value: '—', icon: 'car-outline',     color: 'success' },
    { label: 'Sans carte NFC',  value: '—', icon: 'warning-outline',  color: 'ruby'    },
  ]);

  constructor(
    private employeService: EmployeService,
    private vehiculeService: VehiculeService
  ) {}

  ngOnInit(): void { this.loadStats(); }

  ionViewWillEnter(): void { this.loadStats(); }

  loadStats(): void {
    console.log('[Dashboard] loadStats() appelé');
    forkJoin({
      employes: this.employeService.getAll(),
      vehicules: this.vehiculeService.getAll()
    }).subscribe({
      next: (res) => {
        const employes = res.employes.data ?? [];
        const vehicules = res.vehicules.data ?? [];
        console.log('[Dashboard] Data reçue - employes:', employes.length, 'vehicules:', vehicules.length);

        this.stats$.next([
          { label: 'Total Chauffeurs', value: String(employes.length), icon: 'people-outline', color: 'night' },
          { label: 'Cartes NFC actives', value: String(employes.filter(e => e.hasCTag).length), icon: 'card-outline', color: 'sky' },
          { label: 'Véhicules actifs', value: String(vehicules.filter(v => v.bActif).length), icon: 'car-outline', color: 'success' },
          { label: 'Sans carte NFC', value: String(employes.filter(e => !e.hasCTag).length), icon: 'warning-outline', color: 'ruby' },
        ]);
        console.log('[Dashboard] Stats mises à jour');
      },
      error: (err) => {
        console.error('[Dashboard] Erreur chargement stats', err);
      }
    });
  }
}
