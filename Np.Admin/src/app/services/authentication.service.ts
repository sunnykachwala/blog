import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

import { LoginResponse } from '../types/loginresponse';
import { APIResponse } from '../types/apiresponse';
import { CommonService } from './common.service'
import { AccessRights } from '../types/pageAcees';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private userSubject: BehaviorSubject<LoginResponse | null>;
  public user: Observable<LoginResponse | null>;
  private currentUserEmail: string = "";
  private currentPassword: string = "";
  readonly TOKEN_KEY = 'news_portal_token';
  readonly REFRESHTOKEN_KEY = 'news_portal_refreshtoken';
  readonly USEREMAIL = 'username';
  readonly ACCESSRIGHTS = 'accessRights';

  constructor(
    private router: Router,
    private http: HttpClient,
    private commonService: CommonService
  ) {
    let loginResponse: LoginResponse | null = null;
    const np_refreshToken = localStorage.getItem(this.REFRESHTOKEN_KEY);
    const np_token = localStorage.getItem(this.TOKEN_KEY);
    const userName = localStorage.getItem(this.USEREMAIL);
    if (userName !== null && np_refreshToken !== null && np_token !== null) {
      loginResponse = {
        userGuid: '',
        firstName: '',
        lastName: '',
        twofactorEnabled: true,
        userEmail: userName,
        refreshToken: np_refreshToken,
        jwtToken: np_token,
        role: '',
        permissions:''
      }
    }
    this.userSubject = new BehaviorSubject<LoginResponse | null>(loginResponse);
    this.user = this.userSubject.asObservable();
    console.log('AuthenticationService - Initialized user:', loginResponse);  // Log initialization
  }

  public get userValue(): LoginResponse | null {
    const user = this.userSubject.value;
    console.log('AuthenticationService - Current user value:', user);  // Log user value retrieval
    return user;
  }

  login(loginData:any) {
    return this.http.post<APIResponse<LoginResponse>>(`${environment.apiUrl}Authenticate/`,  loginData , { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            this.currentUserEmail = loginData.userEmail;
            this.currentPassword = loginData.password;

            // Successful response, handle the data here
            if (!response.data.twofactorEnabled) {
              this.userSubject.next(response.data);
              localStorage.setItem(this.USEREMAIL, loginData.serEmail);
              localStorage.setItem(this.TOKEN_KEY, response.data.jwtToken);
              localStorage.setItem(this.REFRESHTOKEN_KEY, response.data.refreshToken);
              this.startRefreshTokenTimer();
            }
            console.log('AuthenticationService - User logged in:', response.data);  // Log successful login
            return response.data;
          } else {
            // Unsuccessful response, handle the error message
            const errorMessage = response.errorMessage || 'An error occurred.';
            return throwError(errorMessage);
          }
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        })
      );
  }

  logout() {
    let token = this.getCookie(this.REFRESHTOKEN_KEY);
    this.stopRefreshTokenTimer();
    this.userSubject.next(null);
    return this.http.post<APIResponse<any>>(`${environment.apiUrl}Authenticate/revoke-token`, { token }, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        })
      );
  }

  refreshToken(): Observable<void>{
 
    const token = localStorage.getItem(this.REFRESHTOKEN_KEY);
    if (!token) {
      console.warn('AuthenticationService - No refresh token found');
      return new Observable<void>((observer) => {
        observer.next();
        observer.complete();
      });
    }
    return this.http.post<any>(`${environment.apiUrl}Authenticate/refresh-token`, { token }, { withCredentials: true }).pipe(
      map((response: any) => {
        if (response.statusCode === 200 && response.data) {
          this.userSubject.next(response.data);
          this.startRefreshTokenTimer();
          console.log('AuthenticationService - Token refreshed successfully:', response.data);

          return response.data;
        }
      }),
      catchError((error: HttpErrorResponse) => {
        let errorMessage = this.commonService.handleHttpError(error);
        console.error('AuthenticationService - Token refresh error:', errorMessage);

        return throwError(errorMessage);
      })
    );
  }

  getCurrentUserEmail() {
    return this.currentUserEmail;
  }

  getCurrentPassword() {
    return this.currentPassword;
  }


  resetQR(userEmail: string, password: string): Observable<APIResponse<any>> {
    return this.http.post(`${environment.apiUrl}Authenticate/reset-qr`, { userEmail, password }, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
          return response;
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        }));
  }

  retrivePassword(userEmail: string): Observable<APIResponse<any>> {
    return this.http.post(`${environment.apiUrl}Authenticate/retrive-password?userEmail=${userEmail}`, {}, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
          return response;
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        }));
  }

  confirmLink(loginGuid: string, confirmationCode: string, isFirstLogin: boolean, resetType: string) {
    const data = {
      loginGuid: loginGuid,
      confirmationCode: confirmationCode,
      isFirstLogin: isFirstLogin,
      resetType: resetType
    };
    return this.http.post(`${environment.apiUrl}Authenticate/confirm-link`, data, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
          return response;
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        }));
  }

  generateQr(userEmail: string, password: string, loginGuid: string, confirmationCode: string, isFirstLogin: boolean) {
    const data = {
      userEmail: userEmail,
      password: password,
      loginGuid: loginGuid,
      confirmationCode: confirmationCode,
      isFirstLogin: isFirstLogin
    };
    const headers = new HttpHeaders().set('X-Source', 'AllowAnonymousComponent');
    return this.http.post(`${environment.apiUrl}Authenticate/generate-qr`, data, { headers, responseType: 'blob', withCredentials: true })
      .pipe(
        map((response: any) => {
          return response;
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        }));
  }

  tokenLogin(userEmail: string, password: string, token: string) {
    var data = {
      userEmail,
      password,
      token
    };
    return this.http.post<APIResponse<LoginResponse>>(`${environment.apiUrl}Authenticate/validate-token-login`, data, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            this.currentUserEmail = userEmail;
            this.currentPassword = password;

            // Successful response, handle the data here          
            this.userSubject.next(response.data);
            this.startRefreshTokenTimer();
            return response.data;
          } else {
            // Unsuccessful response, handle the error message
            const errorMessage = response.errorMessage || 'An error occurred.';
            return throwError(errorMessage);
          }
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        })
      );
  }

  resetPassword(userEmail: string, password: string, loginGuid: string, confirmationCode: string, isFirstLogin: boolean): Observable<APIResponse<any>> {

    const data = {
      loginGuid: loginGuid,
      confirmationCode: confirmationCode,
      isFirstLogin: isFirstLogin,
      password: password,
      resetType: 'Password',
      userEmail: userEmail
    };

    return this.http.post(`${environment.apiUrl}Authenticate/reset-password`, data, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
          return response;
        }),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.commonService.handleHttpError(error);
          return throwError(errorMessage);
        })
      );
  }

  getJwtToken() {
    return localStorage.getItem(this.TOKEN_KEY);
  }
  // helper methods
  private refreshTokenTimeout: any;

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    let response = this.userValue;
    if (response != null) {
      let tokenValue = response.jwtToken?.split('.')[1];
      if (tokenValue) {
        const jwtToken = JSON.parse(atob(tokenValue));
        // set a timeout to refresh the token a minute before it expires
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (60 * 1000);
        this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);

      }
    }
  }

  canAccessPage(routing: string): boolean {
    const questionMarkPosition = routing.indexOf("?");

    if (questionMarkPosition > -1) {
      routing = routing.slice(0, questionMarkPosition);
    }
    const accessRights: AccessRights[] = JSON.parse(localStorage.getItem(this.ACCESSRIGHTS) || '[]');
    const accessrights = accessRights.find((ar) => ar.pageRoute === routing);

    return (accessrights != null && accessrights != undefined);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }

  private getCookie(c_name: string): string {
    if (document.cookie.length > 0) {
      let c_start = document.cookie.indexOf(c_name + "=");
      if (c_start != -1) {
        c_start = c_start + c_name.length + 1;
        let c_end = document.cookie.indexOf(";", c_start);
        if (c_end == -1) {
          c_end = document.cookie.length;
        }
        return unescape(document.cookie.substring(c_start, c_end));
      }
    }
    return "";
  }
}
