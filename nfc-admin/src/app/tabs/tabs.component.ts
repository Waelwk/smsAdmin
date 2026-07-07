import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.scss'],
  host: { 'style': 'display: block; width: 100%; height: 100%;' }
})
export class TabsComponent implements OnInit {
  activeRoute = '';

  tabs = [
    /* { label: 'Dashboard',  icon: 'home-outline',     tab: 'dashboard'  }, */
    { label: 'Chauffeurs', icon: 'people-outline',   tab: 'chauffeurs' },
    { label: 'Véhicules',  icon: 'car-outline',      tab: 'vehicules'  },
   /*  { label: 'Cartes NFC', icon: 'card-outline',     tab: 'cartes'     }, */
    { label: 'Responsable', icon: 'id-card-outline',  tab: 'creation-carte-responsable' },
    { label: 'Paramètres', icon: 'settings-outline', tab: 'parametres' },
  ];

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe((e: any) => { this.activeRoute = e.urlAfterRedirects; });
  }

  isActive(tab: string): boolean {
    return this.activeRoute.startsWith('/' + tab);
  }

  navigate(tab: string): void {
    this.router.navigate(['/' + tab]);
  }
}
