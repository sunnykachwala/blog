import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { environment } from '../environments/environment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Get the token from your preferred authentication service
    const authToken = this.authenticationService.getJwtToken() as string;
    var token: string = '';
    const isApiUrl = request.url.startsWith(environment.apiUrl);
    const user = this.authenticationService.userValue;
    const source = request.headers.get('X-Source');
    var headers = new HttpHeaders();
    const authRequest = request.clone({ headers });
 
    if (user === null) {
      if (isApiUrl) {
        //request = request.clone({
        //  setHeaders: { Authorization: `Bearer ${token}` }
        ////});
        //headers = request.headers.set('Authorization', `Bearer ${token}`);
        const modifiedRequest = authRequest.clone({
          headers: authRequest.headers.set('Authorization', `Bearer ${token} `)
        });
        return next.handle(modifiedRequest);
      }
    } else {
      const isLoggedIn = user && user?.jwtToken
        ;
      if (isLoggedIn && isApiUrl) {
        if (source !== 'AllowAnonymousComponent') {
          token = user?.jwtToken;
          const modifiedRequest = authRequest.clone({
            headers: authRequest.headers.set('Authorization', `Bearer ${token} `)
          });
          return next.handle(modifiedRequest);
        }
      }
      else {
        if (source !== 'AllowAnonymousComponent') {
          const modifiedRequest = authRequest.clone({
            headers: authRequest.headers.set('Authorization', `Bearer ${token} `)
          });
          return next.handle(modifiedRequest);
        }
      }
    }
    // Pass the cloned request to the next interceptor or the HTTP handler
    return next.handle(authRequest);
  }
  getJwtToken() {
    // Decode user data from localStorage
    const decodedUser = localStorage.getItem("news_portal_User")
      ? JSON.parse(localStorage.getItem("news_portal_User") ?? "")
      : null;

    // Initialize user data if available
    if (decodedUser) {
      return decodedUser.token;
    }
    else {
      return "";
    }
  }
}
