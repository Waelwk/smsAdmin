import { Component, OnInit } from '@angular/core';
import { ApiConfigService } from '../../core/services/api-config.service';

@Component({
  selector: 'app-parametres',
  templateUrl: './parametres.page.html'
})
export class ParametresPage implements OnInit {
  ip = '';
  port = '';
  currentApiUrl = '';

  constructor(private apiConfig: ApiConfigService) {}

  ngOnInit(): void {
    const config = this.apiConfig.config;
    this.ip = config.ip;
    this.port = config.port;
    this.updateCurrentUrl();
    this.apiConfig.configUpdates.subscribe(() => this.updateCurrentUrl());
  }

  onIpInput(event: any): void {
    this.ip = String(event.target.value ?? '');
  }

  onPortInput(event: any): void {
    this.port = String(event.target.value ?? '');
  }

  save(): void {
    if (!this.validate()) {
      return;
    }

    this.apiConfig.save(this.ip, this.port);
    this.updateCurrentUrl();
    alert('Configuration enregistrée avec succès.');
  }

  private validate(): boolean {
    const ip = this.ip.trim();
    const port = this.port.trim();

    if (!ip) {
      alert('L\'adresse IP est requise.');
      return false;
    }

    if (!port || isNaN(Number(port)) || Number(port) < 1 || Number(port) > 65535) {
      alert('Le port doit être un nombre compris entre 1 et 65535.');
      return false;
    }

    return true;
  }

  private updateCurrentUrl(): void {
    this.currentApiUrl = this.apiConfig.getBase();
  }
}
