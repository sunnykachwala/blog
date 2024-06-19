import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })

export class AuthGuard implements CanActivate {
  private user: any;
  private permissions: string[] | null = null;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    console.log('CanActivate called');
    const isLoggedIn = this.IsAuthenticated();
    const requiredPermission = next.data['permission'] as string;

    if (isLoggedIn) {
      this.initializePermissions();

      if (requiredPermission === "MyProfile") {
        return true;
      }

      if (this.permissions) {
        return this.permissions.includes(requiredPermission);
      } else {
        // If permissions are not loaded yet, redirect to login or handle appropriately
        this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
    } else {
      if (requiredPermission === "Login" || requiredPermission === "ForgotPassword" || requiredPermission === "ResetPassword") {
        return true;
      }
      this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
  }

  private IsAuthenticated(): boolean {
    const user = this.authenticationService.userValue;
    return user ? !!localStorage.getItem("news_portal__User") : false;
  }

  private initializePermissions() {
    if (this.permissions === null) {
      const storedUser = localStorage.getItem("news_portal__User");
      if (storedUser) {
        const decodedUser = JSON.parse(storedUser);
        this.permissions = decodedUser.permissions.split(",").map((permission: string) => permission.trim());
      }
    }
  }
}
