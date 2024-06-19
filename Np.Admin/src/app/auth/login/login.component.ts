import { Component, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import { FormGroup, NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { CommonService } from '../../services/common.service';
import { first } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});

  returnUrl: string = '';
  error = '';

  public isFormSubmitted = false;
  public isFormValidated = false;
  public isTwoFactorEnabled = false;
  public isTwoFactorEmailSent = false;
  private loginData = {
    organisationName: '',
    userEmail: '',
    password: '',
    passCode: '',
    rememberMe: false,
  }
  public languageModal = {
    show: false,
    loading: false
  }
  constructor(private http: HttpClient, public app: AppService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private commonService: CommonService) {
    // redirect to home if already logged in
    if (this.authService.userValue) {
      let returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
      this.router.navigate([returnUrl]);
    }
  }
  ngOnInit() {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  // submit login form
  submitForm(form: NgForm) {
    this.isFormSubmitted = true;
    this.isFormValidated = true;
    if (form.valid) {
      this.loginData.organisationName = "KampherTech";
      this.loginData.userEmail = form.value.email;
      this.loginData.password = form.value.password;
      this.loginData.rememberMe = form.value.isRemember == true ? true : false;
      console.log(this.loginData);
      // HTTP POST request to login endpoint
      this.authService.login(this.loginData)
        .pipe(first())
        .subscribe({
          next: (response: any) => {
            if (response.twofactorEnabled) {
              this.router.navigate(['validate-token']);
            }
            else if (response.jwtToken !== null || response.jwtToken !== undefined) {
              this.app.SuccessMessage(this.app.Localize("Success!"), this.app.Localize("You have been successfully logged in."));
              this.app.Login(response, this.returnUrl);
              localStorage.setItem(this.authService.TOKEN_KEY, response.jwtToken);
              localStorage.setItem(this.authService.REFRESHTOKEN_KEY, response.refreshToken);
              localStorage.setItem(this.authService.USEREMAIL, this.loginData.userEmail);
              this.router.navigate([]);
            }
            else {
              this.commonService.myTimer();
              this.error = response.errorMessage;
              this.isFormSubmitted = false;
              this.commonService.openDialog(this.error);
            }
          },
          error: (error: any) => {
            this.commonService.myTimer();
            this.error = error;
            this.commonService.openDialog(error);
            this.isFormSubmitted = false;
          }
        }).add(() => {
          this.isFormSubmitted = false;
        });
      this.commonService.myTimer();
      //this.http.post<any>('api/auth/login', this.loginData).subscribe({
      //  next: data => {
      //    if ('isEmailSent' in data) {
      //      this.isTwoFactorEnabled = true;
      //      this.isTwoFactorEmailSent = true;
      //      this.isFormSubmitted = false;
      //      this.isFormValidated = false;
      //    }
      //    else {
      //      this.app.SuccessMessage(this.app.Localize("Success!"), this.app.Localize("You have been successfully logged in."));
      //      this.app.Login(data);
      //      this.isFormSubmitted = false;
      //      this.isFormValidated = false;
      //    }
      //  },
      //  error: error => {
      //    this.app.ErrorMessage(this.app.Localize("Error!"), this.app.Localize("Enter valid credentials."));
      //    this.isFormSubmitted = false;
      //    this.isFormValidated = false;
      //  },
      //});
    }
    else {
      this.isFormSubmitted = false;
    }
  }

  submitForm1(form: NgForm) {
    this.isFormSubmitted = true;
    this.isFormValidated = true;
    if (form.valid) {
      this.loginData.passCode = form.value.passCode;
      // HTTP POST request to login endpoint
      this.http.post<any>('api/auth/login', this.loginData).subscribe({
        next: data => {
          this.app.SuccessMessage(this.app.Localize("Success!"), this.app.Localize("You have been successfully logged in."));
          this.app.Login(data, this.returnUrl);
          this.isFormSubmitted = false;
          this.isFormValidated = false;
        },
        error: error => {
          this.app.ErrorMessage(this.app.Localize("Error!"), this.app.Localize("Enter valid pass code."));
          this.isFormSubmitted = false;
          this.isFormValidated = false;
        },
      });
    }
    else {
      this.isFormSubmitted = false;
    }
  }

  public resetPage() {
    this.isFormSubmitted = false;
    this.isFormValidated = false;
    this.isTwoFactorEnabled = false;
    this.isTwoFactorEmailSent = false;

    this.loginData = {
      organisationName: '',
      userEmail: '',
      password: '',
      passCode: '',
      rememberMe: false,
    }
  }
}
