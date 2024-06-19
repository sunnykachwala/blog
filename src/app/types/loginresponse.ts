export class LoginResponse {
  userGuid: string = '';
  firstName: string = '';
  lastName: string = '';
  twofactorEnabled: boolean = true;
  userEmail: string = '';
  refreshToken: string = '';
  jwtToken: string = '';
  role: string = '';
  permissions: string = '';
}
