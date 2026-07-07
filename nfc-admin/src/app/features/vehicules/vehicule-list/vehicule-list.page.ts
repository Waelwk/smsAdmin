import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { Router } from '@angular/router';
import { VehiculeService } from '../../../core/services/vehicule.service';
import { VehiculeList } from '../../../core/models/vehicule.model';

@Pipe({ name: 'withVCTag' })
export class WithVCTagPipe implements PipeTransform {
  transform(items: VehiculeList[]): number { return items.filter(i => i.hasCTag).length; }
}

@Pipe({ name: 'noVCTag' })
export class NoVCTagPipe implements PipeTransform {
  transform(items: VehiculeList[]): number { return items.filter(i => !i.hasCTag).length; }
}

@Component({
  selector: 'app-vehicule-list',
  templateUrl: './vehicule-list.page.html'
})
export class VehiculeListPage implements OnInit {
  vehicules: VehiculeList[] = [];
  keyword = '';
  loading = false;
  error = '';

  constructor(private vehiculeService: VehiculeService, private router: Router) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.error = '';
    this.vehiculeService.search(this.keyword).subscribe({
      next: res => { this.vehicules = res.data ?? []; this.loading = false; },
      error: err => { this.error = err.message ?? 'Erreur'; this.loading = false; }
    });
  }

  onSearch(event: Event): void {
    this.keyword = (event.target as HTMLInputElement).value.trim();
    this.load();
  }

  goToDetail(id: string): void { this.router.navigate(['/vehicules', id]); }
}
