import { Injectable } from '@angular/core';
import { HttpClient, HttpHandler, HttpXhrBackend, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorInterceptor } from './error.interceptor';

@Injectable()
export class IonicHttpClient extends HttpClient {
  constructor(handler: HttpHandler) {
    super(handler);
  }
}

export { HttpErrorResponse };

export const HTTP_CLIENT_PROVIDERS = [
  { provide: HttpClient, useClass: IonicHttpClient },
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
];
