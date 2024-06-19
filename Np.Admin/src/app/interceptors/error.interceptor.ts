import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingService } from '../services/load.service';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private totalRequests = 0;
  constructor(private authenticationService: AuthenticationService,
    public loadingService: LoadingService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.totalRequests++;
    setTimeout(() => {
      this.loadingService.setLoading(true);
    }, 0);

    return next.handle(request)
      .pipe(
        finalize(() => {
          this.totalRequests--;
          if (this.totalRequests === 0) {
            setTimeout(() => {
              this.loadingService.setLoading(false);
            }, 0);
          }
        }),
        catchError(err => {
          if ([401, 403].includes(err.status) && this.authenticationService.userValue) {
            // auto logout if 401 or 403 response returned from api
            this.authenticationService.logout();
          }
          //const error = (err && err.error && err.error.data) || err.statusText;    
          return throwError(err);
        }))
  }
}
