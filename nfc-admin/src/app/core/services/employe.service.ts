import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfigService } from './api-config.service';
import { ApiResponse } from '../models/api-response.model';
import { EmployeList, EmployeDetail } from '../models/employe.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class EmployeService {
  constructor(private http: HttpClient, private apiConfig: ApiConfigService) {}

  private get base(): string {
    const apiUrl = this.apiConfig.getBase();
    return apiUrl ? `${apiUrl.replace(/\/$/, '')}/employes` : `${environment.apiUrl}/employes`;
  }

  getAll(): Observable<ApiResponse<EmployeList[]>> {
    return this.http.get<ApiResponse<EmployeList[]>>(this.base);
  }

  getById(matricule: string): Observable<ApiResponse<EmployeDetail>> {
    return this.http.get<ApiResponse<EmployeDetail>>(`${this.base}/${matricule}`);
  }

  search(keyword: string): Observable<ApiResponse<EmployeList[]>> {
    return this.http.get<ApiResponse<EmployeList[]>>(`${this.base}/search`, {
      params: { keyword }
    });
  }

  generatePassword(matricule: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      `${this.base}/${matricule}/generate-password`, {}
    );
  }

  generateCTag(matricule: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(
      `${this.base}/${matricule}/generate-ctag`, {}
    );
  }

  updateCTag(matricule: string, ctag: string): Observable<ApiResponse<null>> {
    return this.http.put<ApiResponse<null>>(
      `${this.base}/${matricule}/ctag`, { cTag: ctag }
    );
  }
}
