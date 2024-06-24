import { AfterViewInit, Component, Inject, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-appsetting',
  templateUrl: './appsetting.component.html',
  styleUrls: ['./appsetting.component.css']
})
export class AppSettingComponent implements OnInit, AfterViewInit {
  // Tab flags
  public tabs = {
    general: false,
    system: false,
    email: false,
    pos: false,
    theme: false
  };

  // General Form Validation and Submission
  isGeneralFormValidated = false;
  isGeneralFormSubmitted = false;
  isGeneralFormLoading = true;

  // System Form Validation and Submission
  isSystemFormValidated = false;
  isSystemFormSubmitted = false;
  isSystemFormLoading = true;

  // Email Form Validation and Submission
  isEmailFormValidated = false;
  isEmailFormSubmitted = false;
  isEmailFormLoading = true;

  // POS Form Validation and Submission
  isPOSFormValidated = false;
  isPOSFormSubmitted = false;
  isPOSFormLoading = true;

  // Theme Form Validation and Submission
  isThemeFormValidated = false;
  isThemeFormSubmitted = false;
  isThemeFormLoading = true;

  // Setting data
  generalSetting: any;
  systemSetting: any;
  emailSetting: any;
  posSetting: any;
  themeSetting: any;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public app: AppService
  ) {

  }

  ngAfterViewInit(): void { }

  ngOnInit(): void {
    this.app.InitTooltip();
    this.fetchGeneralSetting();
  }

  // Reset tabs to false
  private ResetTabs() {
    this.tabs = {
      general: false,
      system: false,
      email: false,
      pos: false,
      theme: false
    };
  }

  // Method to load and display an image in the form
  private loadImage(imageUrl: string | null, property: string) {
    if (imageUrl != null) {
      // Load the image data from the server
      this.http.get(imageUrl, { responseType: 'blob' }).subscribe((response) => {
        const reader = new FileReader();
        reader.addEventListener('load', () => {
          const r = reader.result as string;
          this.generalSetting[property] = r; // Display the image
        }, false);

        if (response) {
          reader.readAsDataURL(response);
        }
      });
    } else {
      this.generalSetting[property] = '/assets/images/default.png'; // Display the default image if no image URL is provided
    }
  }

  // Event handlers for Logo and AppIcon file inputs
  onLogoFileChange(event: any) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput && fileInput.files) {
      const file = fileInput.files[0];
      this.generalSetting.settings.logoFile = file;
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          if (typeof reader.result === 'string') {
            this.generalSetting.settings.logo = reader.result;
          }
        };
        reader.readAsDataURL(file);
      }
    }
  }

  onAppIconFileChange(event: any) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput && fileInput.files) {
      const file = fileInput.files[0];
      this.generalSetting.settings.appIconFile = file;
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          if (typeof reader.result === 'string') {
            this.generalSetting.settings.appIcon = reader.result;
          }
        };
        reader.readAsDataURL(file);
      }
    }
  }

  // Load General Setting Form Data
  public fetchGeneralSetting() {
    this.ResetTabs();
    this.tabs.general = true;
    if (this.isGeneralFormLoading) {
      this.http.get<any>(this.baseUrl + 'api/settings/general').subscribe({
        next: data => {
          this.generalSetting = data;
          data.settings.logo = data.settings?.logo ? `/api/media/generalimage/${data.settings.logo}` : '/assets/images/default.png';
          data.settings.appIcon = data.settings?.appIcon ? `/api/media/generalimage/${data.settings.appIcon}` : '/assets/images/default.png';
          this.loadImage(data.settings?.logo, 'settings.logo');
          this.loadImage(data.settings?.appIcon, 'settings.appIcon');
          this.isGeneralFormLoading = false;
        },
        error: error => {
          this.app.HandleApiError(error);
        },
      });
    }
  }

  // Load System Setting Form Data
  public fetchSystemSetting() {
    this.ResetTabs();
    this.tabs.system = true;
    if (this.isSystemFormLoading) {
      this.http.get<any>(this.baseUrl + 'api/settings/system').subscribe({
        next: data => {
          this.systemSetting = data;
          this.isSystemFormLoading = false;
        },
        error: error => {
          this.app.HandleApiError(error);
        },
      });
    }
  }

  // Load Email Setting Form Data
  public fetchEmailSetting() {
    this.ResetTabs();
    this.tabs.email = true;
    if (this.isEmailFormLoading) {
      this.http.get<any>(this.baseUrl + 'api/settings/email').subscribe({
        next: data => {
          this.emailSetting = data;
          this.isEmailFormLoading = false;
        },
        error: error => {
          this.app.HandleApiError(error);
        },
      });
    }
  }

  // Load POS Setting Form Data
  public fetchPOSSetting() {
    this.ResetTabs();
    this.tabs.pos = true;
    if (this.isPOSFormLoading) {
      this.http.get<any>(this.baseUrl + 'api/settings/pos').subscribe({
        next: data => {
          this.posSetting = data;
          this.isPOSFormLoading = false;
          setTimeout(() => {
            this.app.InitCleaveNumber('.cleave-number');
            this.app.InitIntCleaveNumber('.int-cleave-number');
          }, 500);
        },
        error: error => {
          this.app.HandleApiError(error);
        },
      });
    }
  }

  // Load Theme Setting Form Data
  public fetchThemeSetting() {
    this.ResetTabs();
    this.tabs.theme = true;
    if (this.isThemeFormLoading) {
      this.http.get<any>(this.baseUrl + 'api/settings/theme').subscribe({
        next: data => {
          this.themeSetting = data;
          this.isThemeFormLoading = false;
        },
        error: error => {
          this.app.HandleApiError(error);
        },
      });
    }
  }

  // Submit general setting form
  submitGeneralForm(form: NgForm) {
    if (form.valid) {
      this.isGeneralFormSubmitted = true;
      const formData = new FormData();
      formData.append('appName', form.value.appName);
      formData.append('logo', form.value.logo);
      formData.append('appIcon', form.value.AppIcon);
      formData.append('defaultCustomer', form.value.defaultCustomer);
      formData.append('saleAccount', form.value.saleAccount);
      formData.append('purchaseAccount', form.value.purchaseAccount);
      formData.append('payrollAccount', form.value.payrollAccount);
      formData.append('copyright', form.value.copyright);
      formData.append('companyName', form.value.companyName);
      formData.append('companyEmail', form.value.companyEmail);
      formData.append('companyPhone', form.value.companyPhone);
      formData.append('companyAddress', form.value.companyAddress);
      formData.append('companyCity', form.value.companyCity);
      formData.append('companyState', form.value.companyState);
      formData.append('companyPostalCode', form.value.companyPostalCode);
      formData.append('companyCountry', form.value.companyCountry);
      formData.append('companyTaxNumber', form.value.companyTaxNumber);
      if (this.generalSetting.settings.logoFile) {
        formData.append('logoFile', this.generalSetting.settings.logoFile, this.generalSetting.settings.logoFile.name);
      }
      if (this.generalSetting.settings.appIconFile) {
        formData.append('appIconFile', this.generalSetting.settings.appIconFile, this.generalSetting.settings.appIconFile.name);
      }

      // Send the form data to the server
      this.http.post<any>(this.baseUrl + 'api/settings/general', formData).subscribe({
        next: data => {
          this.app.SuccessMessage(this.app.Localize('Success!'), this.app.Localize('General setting updated successfully.'));
          this.isGeneralFormSubmitted = false;
          this.isGeneralFormValidated = false;
          this.app.ReloadAppConfig();
        },
        error: error => {
          this.app.HandleApiError(error);
          this.isGeneralFormSubmitted = false;
          this.isGeneralFormValidated = false;
        }
      });

    } else {
      this.isGeneralFormValidated = true;
    }
  }

  // Submit system setting form
  submitSystemForm(form: NgForm) {
    if (form.valid) {
      this.isSystemFormSubmitted = true;

      // Send the form data to the server
      this.http.post<any>(this.baseUrl + 'api/settings/system', form.value).subscribe({
        next: data => {
          this.app.SuccessMessage(this.app.Localize('Success!'), this.app.Localize('System setting updated successfully.'));
          this.isSystemFormSubmitted = false;
          this.isSystemFormValidated = false;
          this.app.Logout();
        },
        error: error => {
          this.app.HandleApiError(error);
          this.isSystemFormSubmitted = false;
          this.isSystemFormValidated = false;
        }
      });

    } else {
      this.isSystemFormValidated = true;
    }
  }

  // Submit email setting form
  submitEmailForm(form: NgForm) {
    if (form.valid) {
      this.isEmailFormSubmitted = true;

      // Send the form data to the server
      this.http.post<any>(this.baseUrl + 'api/settings/email', form.value).subscribe({
        next: data => {
          this.app.SuccessMessage(this.app.Localize('Success!'), this.app.Localize('Email setting updated successfully.'));
          this.isEmailFormSubmitted = false;
          this.isEmailFormValidated = false;
        },
        error: error => {
          this.app.HandleApiError(error);
          this.isEmailFormSubmitted = false;
          this.isEmailFormValidated = false;
        }
      });

    } else {
      this.isEmailFormValidated = true;
    }
  }

  // Submit pos setting form
  submitPOSForm(form: NgForm) {
    if (form.valid) {
      this.isPOSFormSubmitted = true;
      // Send the form data to the server
      this.http.post<any>(this.baseUrl + 'api/settings/pos', form.value).subscribe({
        next: data => {
          this.app.SuccessMessage(this.app.Localize('Success!'), this.app.Localize('POS setting updated successfully.'));
          this.isPOSFormSubmitted = false;
          this.isPOSFormValidated = false;
        },
        error: error => {
          this.app.HandleApiError(error);
          this.isPOSFormSubmitted = false;
          this.isPOSFormValidated = false;
        }
      });

    } else {
      this.isEmailFormValidated = true;
    }
  }

  // Submit theme setting form
  submitThemeForm(form: NgForm) {
    if (form.valid) {
      this.isThemeFormSubmitted = true;

      // Send the form data to the server
      this.http.post<any>(this.baseUrl + 'api/settings/theme', form.value).subscribe({
        next: data => {
          this.app.SuccessMessage(this.app.Localize('Success!'), this.app.Localize('Theme setting updated successfully.'));
          this.isThemeFormSubmitted = false;
          this.isThemeFormValidated = false;
          localStorage.removeItem("goposify_AppConfig");
          window.location.reload();
        },
        error: error => {
          this.app.HandleApiError(error);
          this.isThemeFormSubmitted = false;
          this.isThemeFormValidated = false;
        }
      });

    } else {
      this.isEmailFormValidated = true;
    }
  }
}
