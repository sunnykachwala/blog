import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppService } from '../../services/app.service';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-myprofile',
  templateUrl: './myprofile.component.html',
  styleUrls: ['./myprofile.component.css']
})
export class MyProfileComponent implements OnInit {
  public isLoading = false;
  public isSubmitted = false;
  public isValidated = false;
  public isImageSubmitted = false;
  public isTwoFactorSubmitted = false;

  public changePassword: any = {};
  public user: any = {
    fullName: '',
    avatarUrl: '',
    avatarFile: null,
    userName: '',
    email: '',
    phone: '',
    isTwoFactorEnabled: false
  };

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public app: AppService
  ) { }

  ngOnInit(): void {
    this.app.InitTooltip();
    this.resetForm();
    this.loadUser();
  }

  // Load user data from the server
  private loadUser() {
    this.http.get<any>(this.baseUrl + 'api/auth/myprofile').subscribe({
      next: data => {
        this.loadFormImage(data?.avatarUrl ? `${this.baseUrl}api/media/userimage/${data.avatarUrl}` : null);
        this.user = data;
        this.isLoading = false;
      },
      error: error => {
        if (error.status == 401) {
          this.app.Logout();
        }
        this.isLoading = false;
      },
    });
  }

  // Reset form data
  private resetForm() {
    this.changePassword = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: '',
    };
    this.isSubmitted = false;
    this.isValidated = false;
    this.isLoading = false;
    this.isTwoFactorSubmitted = false;
    this.isImageSubmitted = false;
    this.loadFormImage(null);
  }

  // Submit the main form
  public submitForm(form: NgForm): void {
    if (form.valid) {
      // Check password match
      if (this.changePassword.newPassword != this.changePassword.confirmPassword) {
        this.app.ErrorMessage(this.app.Localize('Error!'), this.app.Localize('The passwords do not match.'));
        return;
      }
      // Change password API call
      this.http.post<any>(this.baseUrl + `api/auth/changepassword`, { ...form.value }).subscribe({
        next: (responseData: any) => {
          this.app.SuccessMessage(this.app.Localize("Success!"), "Password changed successfully.");
          this.resetForm();
        },
        error: (error) => {
          this.app.HandleApiError(error);
          this.isSubmitted = false;
        },
      });
    } else {
      this.isValidated = true;
    }
  }

  // Load and display user's profile image
  private loadFormImage(imageUrl: string | null) {
    if (imageUrl != null) {
      this.http.get(imageUrl, { responseType: 'blob' }).subscribe((response) => {
        const reader = new FileReader();

        reader.addEventListener('load', () => {
          const r = reader.result as string;
          this.user.avatarUrl = r;
        }, false);

        if (response) {
          reader.readAsDataURL(response);
        }
      });
    } else {
      this.user.avatarUrl = '/assets/images/default.png';
    }
  }

  // Handle changes to the file input for the profile image
  public onFileInputChange(event: Event) {
    this.isImageSubmitted = true;
    const fileInput = event.target as HTMLInputElement;
    if (fileInput && fileInput.files) {
      this.user.avatarFile = fileInput.files[0];
      if (this.user.avatarFile) {
        const reader = new FileReader();
        reader.onload = (e) => {
          if (typeof reader.result === 'string') {
            this.user.avatarUrl = reader.result;
          }
        };
        reader.readAsDataURL(this.user.avatarFile);

        // Upload profile image API call
        const formData = new FormData();
        formData.append('image', this.user.avatarFile, this.user.avatarFile.name);
        this.http.post<any>(this.baseUrl + `api/auth/updateprofileimage`, formData).subscribe({
          next: (responseData) => {
            this.app.SuccessMessage(this.app.Localize("Success!"), this.app.Localize('Profile image uploaded successfully.'));
            this.isImageSubmitted = false;
          },
          error: (error) => {
            this.app.HandleApiError(error);
            this.isImageSubmitted = false;
          },
        });
      }
    }
  }

  // Handle changes to two-factor authentication
  public onTwoFactorChange() {
    this.isTwoFactorSubmitted = true;
    // Update two-factor authentication API call
    this.http.get<any>(this.baseUrl + `api/auth/updatetwofactorauth`).subscribe({
      next: (responseData) => {
        this.app.SuccessMessage(this.app.Localize("Success!"), this.app.Localize('Two factor authentication updated successfully.'));
        this.isTwoFactorSubmitted = false;
      },
      error: (error) => {
        this.app.HandleApiError(error);
        this.user.isTwoFactorEnabled = !this.user.isTwoFactorEnabled;
        this.isTwoFactorSubmitted = false;
      },
    });
  }
}
