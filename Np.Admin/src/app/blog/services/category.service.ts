import { Injectable } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { APIResponse } from '../../types/apiresponse';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  get(filter: any) {
    return this.http.get<APIResponse<any>>(`${environment.apiUrl}category/get-all?Page=${filter.page}&PageSize=${filter.pageSize}&IsActive=${filter.isActive}&Search=${filter.search}`,
      { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          return throwError(error);
        })
      );
  }

  add(category: any) {
    return this.http.post<APIResponse<any>>(`${environment.apiUrl}category/create/`, category, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          return throwError(error);
        })
      );
  }
}
