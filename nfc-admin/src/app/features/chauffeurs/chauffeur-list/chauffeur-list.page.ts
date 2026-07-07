import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { Router } from '@angular/router';
import { EmployeService } from '../../../core/services/employe.service';
import { EmployeList } from '../../../core/models/employe.model';

@Pipe({ name: 'withCTag' })
export class WithCTagPipe implements PipeTransform {
  transform(items: EmployeList[]): number { return items.filter(i => i.hasCTag).length; }
}

@Pipe({ name: 'withPassword' })
export class WithPasswordPipe implements PipeTransform {
  transform(items: EmployeList[]): number { return items.filter(i => i.hasPassword).length; }
}

@Pipe({ name: 'noCTag' })
export class NoCTagPipe implements PipeTransform {
  transform(items: EmployeList[]): number { return items.filter(i => !i.hasCTag).length; }
}

@Component({
  selector: 'app-chauffeur-list',
  templateUrl: './chauffeur-list.page.html'
})
export class ChauffeurListPage implements OnInit {
  chauffeurs: EmployeList[] = [];
  keyword = '';
  loading = false;
  error = '';

  constructor(private employeService: EmployeService, private router: Router) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.error = '';
    this.employeService.search(this.keyword).subscribe({
      next: res => { this.chauffeurs = res.data ?? []; this.loading = false; },
      error: err => { this.error = err.message ?? 'Erreur de chargement.'; this.loading = false; }
    });
  }

  onSearch(event: Event): void {
    this.keyword = (event.target as HTMLInputElement).value.trim();
    this.load();
  }

  goToDetail(matricule: string): void { this.router.navigate(['/chauffeurs', matricule]); }
}
