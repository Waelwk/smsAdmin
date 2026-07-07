import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfigService } from './api-config.service';
import { ApiResponse } from '../models/api-response.model';
import { VehiculeList, VehiculeDetail } from '../models/vehicule.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class VehiculeService {
  constructor(private http: HttpClient, private apiConfig: ApiConfigService) {}

  private get base(): string {
    const apiUrl = this.apiConfig.getBase();
    return apiUrl ? `${apiUrl.replace(/\/$/, '')}/vehicules` : `${environment.apiUrl}/vehicules`;
  }

  getAll(): Observable<ApiResponse<VehiculeList[]>> {
    return this.http.get<ApiResponse<VehiculeList[]>>(this.base);
  }

  getById(id: string): Observable<ApiResponse<VehiculeDetail>> {
    return this.http.get<ApiResponse<VehiculeDetail>>(`${this.base}/${id}`);
  }

  search(keyword: string): Observable<ApiResponse<VehiculeList[]>> {
    return this.http.get<ApiResponse<VehiculeList[]>>(`${this.base}/search`, {
      params: { keyword }
    });
  }

  generateCTag(id: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.base}/${id}/generate-ctag`, {});
  }

  updateCTag(id: string, ctag: string): Observable<ApiResponse<null>> {
    return this.http.put<ApiResponse<null>>(`${this.base}/${id}/ctag`, { cTag: ctag });
  }
}
