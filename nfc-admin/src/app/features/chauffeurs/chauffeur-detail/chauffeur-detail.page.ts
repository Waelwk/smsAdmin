import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeService } from '../../../core/services/employe.service';
import { CarteService } from '../../../core/services/carte.service';
import { EmployeDetail } from '../../../core/models/employe.model';

@Component({
  selector: 'app-chauffeur-detail',
  templateUrl: './chauffeur-detail.page.html'
})
export class ChauffeurDetailPage implements OnInit {
  chauffeur: EmployeDetail | null = null;
  loading = false;
  ctagManuel = '';
  showCtagInput = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employeService: EmployeService,
    private carteService: CarteService
  ) {}

  ngOnInit(): void {
    const matricule = this.route.snapshot.paramMap.get('matricule')!;
    this.load(matricule);
  }

  load(matricule: string): void {
    this.loading = true;
    this.employeService.getById(matricule).subscribe({
      next: res => { this.chauffeur = res.data; this.loading = false; },
      error: () => this.loading = false
    });
  }

  generatePassword(): void {
    if (!this.chauffeur) return;
    this.employeService.generatePassword(this.chauffeur.matricule).subscribe({
      next: res => {
        alert(`Mot de passe généré : ${res.data}\n\nNotez ce mot de passe, il ne sera plus affiché.`);
        this.load(this.chauffeur!.matricule);
      },
      error: err => alert(`Erreur : ${err.message}`)
    });
  }

  generateCTag(): void {
    if (!this.chauffeur) return;
    this.employeService.generateCTag(this.chauffeur.matricule).subscribe({
      next: () => this.load(this.chauffeur!.matricule),
      error: err => alert(`Erreur : ${err.message}`)
    });
  }

  saveCTagManuel(): void {
    if (!this.chauffeur || !this.ctagManuel.trim()) return;
    this.employeService.updateCTag(this.chauffeur.matricule, this.ctagManuel.trim()).subscribe({
      next: () => {
        this.showCtagInput = false;
        this.ctagManuel = '';
        this.load(this.chauffeur!.matricule);
      },
      error: err => alert(`Erreur : ${err.message}`)
    });
  }

  creerCarte(): void {
    if (!this.chauffeur) return;
    this.carteService.getCarteChauffeur(this.chauffeur.matricule).subscribe({
      next: res => this.router.navigate(
        ['/chauffeurs', this.chauffeur!.matricule, 'nfc'],
        { state: { nfcData: res.data } }
      ),
      error: err => alert(`Erreur : ${err.message}`)
    });
  }
}
