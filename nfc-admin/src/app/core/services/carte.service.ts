import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfigService } from './api-config.service';
import { ApiResponse } from '../models/api-response.model';
import { NfcData } from '../models/nfc-data.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CarteService {
  constructor(private http: HttpClient, private apiConfig: ApiConfigService) {}

  private get base(): string {
    const apiUrl = this.apiConfig.getBase();
    return apiUrl ? `${apiUrl.replace(/\/$/, '')}/cartes` : `${environment.apiUrl}/cartes`;
  }

  getCarteChauffeur(matricule: string): Observable<ApiResponse<NfcData>> {
    return this.http.post<ApiResponse<NfcData>>(`${this.base}/chauffeurs`, { matricule });
  }

  getCarteVehicule(id: string): Observable<ApiResponse<NfcData>> {
    return this.http.post<ApiResponse<NfcData>>(`${this.base}/vehicules`, { id });
  }
}
