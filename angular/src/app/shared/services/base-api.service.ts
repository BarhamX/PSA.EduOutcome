import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigStateService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root'
})
export class BaseApiService {
  protected apiUrl: string;

  constructor(
    protected http: HttpClient,
    protected configService: ConfigStateService
  ) {
    this.apiUrl = this.configService.getApiUrl();
  }

  protected buildParams(params?: any): HttpParams {
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined && params[key] !== '') {
          if (Array.isArray(params[key])) {
            params[key].forEach((item: any) => {
              httpParams = httpParams.append(key, item.toString());
            });
          } else {
            httpParams = httpParams.set(key, params[key].toString());
          }
        }
      });
    }

    return httpParams;
  }

  protected get<T>(url: string, params?: any): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}${url}`, {
      params: this.buildParams(params)
    });
  }

  protected post<T>(url: string, body?: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}${url}`, body);
  }

  protected put<T>(url: string, body?: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}${url}`, body);
  }

  protected delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}${url}`);
  }
}