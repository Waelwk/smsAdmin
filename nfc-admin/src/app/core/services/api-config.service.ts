import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';

const STORAGE_KEY = 'api_config';

export interface ApiConfig {
  ip: string;
  port: string;
}

@Injectable({ providedIn: 'root' })
export class ApiConfigService {
  private readonly config$ = new BehaviorSubject<ApiConfig>(this.load());

  get config() {
    return this.config$.value;
  }

  get configUpdates() {
    return this.config$.asObservable();
  }

  getBase(): string {
    const configured = this.getApiUrl();
    return configured ? configured : environment.apiUrl;
  }

  getApiUrl(): string {
    const { ip, port } = this.config$.value;
    const trimmedIp = ip?.trim();
    const trimmedPort = port?.trim();

    if (!trimmedIp || !trimmedPort) {
      return '';
    }

    return `http://${trimmedIp}:${trimmedPort}/api`;
  }

  save(ip: string, port: string): void {
    this.config$.next({ ip: ip.trim(), port: port.trim() });
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(this.config$.value));
    } catch {
      // ignore storage errors
    }
  }

  private load(): ApiConfig {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (raw) {
        const parsed = JSON.parse(raw);
        if (parsed.ip || parsed.port) {
          return { ip: parsed.ip ?? '', port: parsed.port ?? '' };
        }
      }
    } catch {
      // ignore parse errors
    }
    return { ip: '', port: '' };
  }
}
