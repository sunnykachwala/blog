import { Injectable } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { APIResponse } from '../../types/apiresponse';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) { }

  get(filter: any) {

    let params = new HttpParams()
    .append('page', `${filter.page}`)
    .append('pageSize', `${filter.pageSize}`)
    .append('Search', `${filter.search}`)
    .append('IsActive', `${filter.isActive}`)
    .append('sortField', `${filter.sortField}`)
    .append('sortOrder', `${filter.sortOrder}`);

    filter.filters.forEach((filter:any) => {
      filter.value.forEach((value:any) => {
        params = params.append(filter.key, value);
      })
    });

    return this.http.get<APIResponse<any>>(`${environment.apiUrl}article/get-all` ,{params}  )
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

  getById(id: any) {
    return this.http.get<APIResponse<any>>(`${environment.apiUrl}article/get/${id}`,
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

  add(article: any) {
    return this.http.post<APIResponse<any>>(`${environment.apiUrl}article/create/`, article, { withCredentials: true })
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
