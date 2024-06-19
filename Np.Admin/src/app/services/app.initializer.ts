import { AuthenticationService } from "./authentication.service";

export function appInitializer(authenticationService: AuthenticationService) {
  return () => new Promise<void>((resolve, reject) => {
    try {
      // attempt to refresh token on app start up to auto authenticate
      authenticationService.refreshToken()
        .subscribe({
          next: (response: any) => {
            console.log('App Initializer - Refresh token successful:', response);
            resolve(response);

            //return response;
          },
          error: (err: any) => {
            console.log('App Initializer - Refresh token failed:', err);
            resolve();
            //return error;
          },
          complete: () => {
            console.log('App Initializer - Refresh token process complete');
            resolve();
          }
        })
        
    }
    catch (ex: any) {
      console.error('App Initializer - Exception occurred:', ex);
      reject(ex);
    }
  });
}
