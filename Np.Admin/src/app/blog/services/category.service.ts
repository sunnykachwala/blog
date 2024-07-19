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
  getFullImageUrl(imagePath: string) {
    // const baseUrl = `${environment.apiUrl}wwwroot/category/images/`; // Replace with your actual base URL
    // return `${baseUrl}${im*******agePath}`;

    return this.http.get(`${environment.apiUrl}category/view?imageName=${imagePath}`, {
      responseType: 'blob'
    }).subscribe();
  }
  getFullImageUrlString(imagePath: string): string {
    const apiBaseUrl = `${environment.apiUrl}wwwroot/category/images/${imagePath}`; // Replace with your actual API base URL
    return `${apiBaseUrl}${imagePath}`;
  }
}
