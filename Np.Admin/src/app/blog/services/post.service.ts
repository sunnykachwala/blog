import { Injectable } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { APIResponse } from '../../types/apiresponse';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) { }

  get(filter: any) {
    return this.http.get<APIResponse<any>>(`${environment.apiUrl}article/get-all/${filter}`,
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
