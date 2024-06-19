import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../environments/environment';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add auth header with jwt if user is logged in and request is to the api url
    const user = this.authenticationService.userValue;
    var token: string = '';
    const isApiUrl = request.url.startsWith(environment.apiUrl);
    const source = request.headers.get('X-Source');
    var headers = new HttpHeaders();
    const authRequest = request.clone({ headers });
    token = this.authenticationService.getJwtToken() as string;
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
    }
    else {
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
    return next.handle(authRequest);
  }


}
