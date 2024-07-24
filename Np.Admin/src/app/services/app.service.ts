import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Route, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { APIResponse } from '../types/apiresponse';
import { environment } from '../environments/environment';
import { catchError, first, map, throwError } from 'rxjs';
import { AuthenticationService } from './authentication.service';

declare var $: any;

// External library declaration
declare let Cleave: any;

@Injectable({
  providedIn: 'root'
})

export class AppService {
  private user: any;
  private userPermissions: any;
  private appConfig: any;
  private stringResources: any;
  private regionName: any;
  private timeZone: any;
  private currencyName: any;
  private languages: any;
  private decimalSeparator: any;
  private groupSeparator: any;
  private currencySymbol: any;

  /*
  * 1. General
  * 2. Format Web API Response Data to local
  * 3. Initialize HTML Form Fields & Format HTML Form Field to APIs & APIs to HTML Form Field
  * 4. User related functions
  * 5. Upload files & replace image src
  */

  constructor(
    private http: HttpClient,
    private router: Router,
    private location: Location,
    private authenticationService: AuthenticationService
  ) {
    // Decode user data from localStorage
    const decodedUser = localStorage.getItem("news_portal_user")
      ? JSON.parse(localStorage.getItem("news_portal_user") ?? "")
      : null;
     
    // Decode app appConfig from localStorage
    const decodedAppConfig = localStorage.getItem("news_portal_AppConfig")
      ? JSON.parse(localStorage.getItem("news_portal_AppConfig") ?? "")
      : null;

    // Decode localization data from localStorage
    const decodedLocalizationData = localStorage.getItem("news_portal_LocalizationData")
      ? JSON.parse(localStorage.getItem("news_portal_LocalizationData") ?? "")
      : null;

    // Initialize user data if available
    if (decodedUser) {
      this.userPermissions = decodedUser.permissions.split(",").map((permission: any) => permission.trim());
      //this.userPermissions = 'Login,Home,Dashboard,MyProfile'.split(",").map((permission: any) => permission.trim());
      this.user = {
        fullName: decodedUser.fullName,
        email: decodedUser.userEmail,
        defaultHome: decodedUser.defaultHome ?? 'Home',
        avatarUrl: decodedUser.avatarUrl ?? '',
        token: decodedUser.jwtToken,
        refreshToken: decodedUser.refreshToken,
      };
    }

    // Initialize app appConfig from localStorage or fetch from API
    if (decodedAppConfig) {
      this.appConfig = decodedAppConfig;
      $('body').attr('class', `${this.appConfig['themeLayout']} ${this.appConfig['themeColor']}`);

      let version = this.GetCookie('news_portal_Version');
      if (version != this.appConfig['version']) {
        localStorage.clear();
        window.location.reload();
      }
    } else {
      this.getApplicationSetting()
        .pipe(first())
        .subscribe({
          next: (response: any) => {
            localStorage.setItem("news_portal_AppConfig", JSON.stringify(response));
            this.appConfig = response;
            $('body').attr('class', `${this.appConfig['themeLayout']} ${this.appConfig['themeColor']}`);
          },
          error: (errorResponse: any) => {
            console.error(errorResponse)
          }
        });
    }

    // Initialize string resources from localStorage or fetch from API
    if (decodedLocalizationData) {
      this.stringResources = decodedLocalizationData.stringResources;
      this.regionName = decodedLocalizationData.regionName;
      this.timeZone = decodedLocalizationData.timeZone;
      this.currencyName = decodedLocalizationData.currencyName;
      this.languages = decodedLocalizationData.languages;
      this.decimalSeparator = decodedLocalizationData.decimalSeparator;
      this.groupSeparator = decodedLocalizationData.groupSeparator;
      this.currencySymbol = decodedLocalizationData.currencySymbol;
      this.ChangeDirection(decodedLocalizationData.isRtl ? 'rtl' : 'ltr');
    } else {
      let languageName = null;
      if (this.GetCookie('language') != null) {
        languageName = this.GetCookie('language');
      } else {
        languageName = 'default';
        this.SetCookie('language', 'default', 30);
      }

      this.getApplicationOtherSetting()
        .pipe(first())
        .subscribe({
          next: (data: any) => {
            this.stringResources = data.stringResources;
            this.regionName = data.regionName;
            this.timeZone = data.timeZone;
            this.currencyName = data.currencyName;
            this.languages = data.languages;
            this.decimalSeparator = this.GetNumberSeparators().decimalSeparator;
            this.groupSeparator = this.GetNumberSeparators().groupSeparator;
            this.currencySymbol = this.GetCurrencySymbol();
            data.decimalSeparator = this.decimalSeparator;
            data.groupSeparator = this.groupSeparator;
            data.currencySymbol = this.currencySymbol;
            localStorage.setItem("news_portal_LocalizationData", JSON.stringify(data));
            this.ChangeDirection(data.isRtl ? 'rtl' : 'ltr');
          },
          error: (errorResponse: any) => {
            console.error(errorResponse)
          }
        });
    }
  }

  private SetCookie(name: any, value: any, daysToExpire: any) {
    const expirationDate = new Date();
    expirationDate.setDate(expirationDate.getDate() + daysToExpire);

    const cookieValue = encodeURIComponent(value) + "; expires=" + expirationDate.toUTCString();
    document.cookie = name + "=" + cookieValue + "; path=/";
  }

  private GetCookie(name: any) {
    const cookieName = name + "=";
    const cookieArray = document.cookie.split(';');

    for (let i = 0; i < cookieArray.length; i++) {
      let cookie = cookieArray[i].trim();
      if (cookie.indexOf(cookieName) === 0) {
        return decodeURIComponent(cookie.substring(cookieName.length, cookie.length));
      }
    }

    return null;
  }

  private GetCurrencySymbol() {
    try {
      const numberFormat = new Intl.NumberFormat(this.regionName, { style: 'currency', currency: this.currencyName });
      const parts = numberFormat.formatToParts(1);
      const currencySymbolPart = parts.find(part => part.type === 'currency');

      if (currencySymbolPart) {
        return currencySymbolPart.value;
      }
      return '';
    } catch (error: any) {
      return '';
    }
  }

  private GetNumberSeparators() {
    let userRegion = this.regionName;
    let numberFormat = new Intl.NumberFormat(userRegion);
    let separators: any = numberFormat.formatToParts(12345.6)
      .find(part => part.type === 'decimal' || part.type === 'group');

    var decimalSeparator = separators.type === 'decimal' ? separators.value : '.';
    var groupSeparator = separators.type === 'group' ? separators.value : ',';

    // Return an object with separators
    return {
      decimalSeparator: decimalSeparator,
      groupSeparator: groupSeparator
    };
  }

  public ReloadAppConfig() {
    this.getApplicationSetting()
      .pipe(first())
      .subscribe({
        next: (response: any) => {
          localStorage.setItem("news_portal_AppConfig", JSON.stringify(response));
          this.appConfig = response;
          $('body').attr('class', `${this.appConfig['themeLayout']} ${this.appConfig['themeColor']}`);
        },
        error: (errorResponse: any) => {
          console.error(errorResponse)
        }
      });
    //this.http.get<any>(`${environment.apiUrl}app/appconfig`)
    //  .subscribe(
    //  data => {
    //    localStorage.setItem("newsletter_AppConfig", JSON.stringify(data.data));
    //      this.appConfig = data.data;
    //    $('body').attr('class', `${this.appConfig['themeLayout']} ${this.appConfig['themeColor']}`);
    //  },
    //  error => console.error(error)
    //);
  }

  public HasPermission(permission: any) {

    return this.userPermissions.includes(permission);
  }

  public HasSomePermission(permission: any) {
    let hasPermission = this.userPermissions.some((userPermission: any) => userPermission.startsWith(permission));
    return hasPermission;
  }
  public HasSomePermissionNew(permission: any) {
    let hasPermission = this.userPermissions.some((userPermission: any) => userPermission.startsWith(permission));
    console.log(hasPermission);
    console.log(permission);
    return hasPermission;
  
  }


  private getApplicationSetting() {
    return this.http.get<APIResponse<any>>(`${environment.apiUrl}app/appconfig`, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            localStorage.setItem("news_portal_AppConfig", JSON.stringify(response.data));
            this.appConfig = response.data;
            this.SetCookie('news_portal_Version', this.appConfig['version'], 60);
            $('body').attr('class', `${this.appConfig['themeLayout']} ${this.appConfig['themeColor']}`);
            return response.data;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          //let errorMessage = this.commonService.handleHttpError(error);
          return throwError(error);
        })
      );
  }

  private getApplicationOtherSetting() {

    return this.http.get<APIResponse<any>>(`${environment.apiUrl}app/localizationdata/default`, { withCredentials: true })
      .pipe(
        map((response: any) => {
          if (response.statusCode === 200 && response.data) {
            return response.data;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          //let errorMessage = this.commonService.handleHttpError(error);        
          return throwError(error);
        })
      );
  }

  /*************************************
   * 1. General
   ************************************/

  // Log in the user
  public Login(data: any, returnUrl: string) {

    $(".splash").removeAttr("style");
    // data.permissions = 'Login,Home,Dashboard,MyProfile';
    localStorage.setItem("news_portal_user", JSON.stringify(data));
    this.userPermissions = data.permissions.split(",").map((permission: any) => permission.trim());
    //this.userPermissions = 'Login,Home,Dashboard,MyProfile'.split(",").map((permission: any) => permission.trim());
    this.user = {
      fullName: data.fullName,
      email: data.userEmail,
      defaultHome: data.defaultHome ?? 'Home',
      avatarUrl: data.avatarUrl ?? '',
      token: data.jwtToken,
      refreshToken: data.refreshToken,
    };
    $(".splash").fadeOut("slow");

    if (returnUrl) {
      let returnUrlWithoutSlash = this.removeLeadingSlash(returnUrl);
      const config: Route[] = this.router.config;
      let returnUrlExists = this.checkRouteExists(config, returnUrlWithoutSlash);
      console.log('Return URL:', returnUrlWithoutSlash);
      console.log('Router Configuration:', config);
      console.log('Return URL Exists:', returnUrlExists);

      if (returnUrlExists) {
        this.router.navigate([returnUrl]).then((success: any) => {
          console.log('Navigation Success:', success);
          if (!success) {
            window.location.href = returnUrlWithoutSlash;
          }
        }).catch(err => {
          console.error('Navigation Error:', err);
          window.location.href = '/';
        });
      } else {
        window.location.href = '/';
      }
    } else {
      window.location.href = '/';
    }

  }

  // Log out the user
  public Logout() {
    this.authenticationService.logout();
    $(".splash").removeAttr("style");
    localStorage.clear();
    $(".splash").fadeOut("slow");
    window.location.href = '/';
  }

  // Check if the user is authenticated
  public IsAuthenticated() {
    return !!localStorage.getItem("news_portal_user");
  }

  // Check if the user has permission
  public UserHasPermission(name: string) {
    return this.userPermissions.includes(name);
  }

  // Redirect user based on authentication status
  public Redirect() {
    if (!this.IsAuthenticated()) {
      this.router.navigate(['/auth/login']);
    } else if (this.user.defaultHome == 'Home') {
      this.router.navigate(['/homemenu']);
    } else if (this.user.defaultHome == 'Dashboard') {
      this.router.navigate(['/dashboard']);
    } else if (this.user.defaultHome == 'POS') {
      this.router.navigate(['/sales/pos']);
    }
  }
  // Get the JWT token from user data
  public GetUserAttr(name: string): string {
    return this.user?.[name] ?? "";
  }

  // Get the JWT token from user data
  public GetJwtToken(): string {
    if (this.user && this.user['token'] !== undefined) {
      return this.user['token'];
    }
    return '';
  }

  // Localize text using string resources or return the original text
  public Localize(text: string) {
    return this.stringResources?.[text] ?? text;
  }

  public GetLanguages() {
    return this.languages;
  }

  // Retrieve app configuration value based on key or return an empty string
  public GetAppConfig(key: any) {
    if (this.appConfig && this.appConfig[key] !== undefined) {
      return this.appConfig[key];
    }
    return '';
  }

  // Reloads localization data and updates UI
  public ReloadLocalizationData(name: any) {
    // Show loading splash screen
    $(".splash").removeAttr("style");
    this.getApplicationOtherSetting()
      .pipe(first())
      .subscribe({
        next: (data: any) => {
          $(".splash").fadeOut("slow");
          data.decimalSeparator = this.GetNumberSeparators().decimalSeparator;
          data.groupSeparator = this.GetNumberSeparators().groupSeparator;
          // Update localization data and UI elements
          this.SetCookie('language', name, 30);
          this.stringResources = data.stringResources;
          this.regionName = this.regionName;
          this.timeZone = data.timeZone;
          this.currencyName = data.currencyName;
          this.languages = data.languages;
          this.decimalSeparator = this.GetNumberSeparators().decimalSeparator;
          this.groupSeparator = this.GetNumberSeparators().groupSeparator;
          this.currencySymbol = this.GetCurrencySymbol();
          data.decimalSeparator = this.decimalSeparator;
          data.groupSeparator = this.groupSeparator;
          data.currencySymbol = this.currencySymbol;
          localStorage.removeItem("news_portal_LocalizationData");
          localStorage.setItem("news_portal_LocalizationData", JSON.stringify(data));
          this.ChangeDirection(data.isRtl ? 'rtl' : 'ltr');
          window.location.reload();
        },
        error: (errorResponse: any) => {
          $(".splash").fadeOut("slow");
          console.error(errorResponse)
        }
      });

  }

  // Changes the text direction (left-to-right or right-to-left)
  private ChangeDirection(dir: 'ltr' | 'rtl'): void {
    const attrName = 'bootstrap-lib';
    const originalLink = document.querySelector(`[${attrName}]`);
    const linkAddress = dir === 'ltr'
      ? "/assets/lib/bootstrap/dist/css/bootstrap.min.css"
      : "assets/lib/bootstrap/dist/css/bootstrap.rtl.min.css";

    // Update link and document direction
    (originalLink as HTMLLinkElement).href = linkAddress;
    document.dir = dir;
  }

  // Navigate back using the browser's location history
  public GoBack() {
    this.location.back();
  }

  public GetMonths() {
    const monthList: any[] = [];

    for (let month = 0; month < 12; month++) {
      const date = new Date(2000, month, 1); // Choose any year, as we're only interested in month names
      const monthName = date.toLocaleDateString(this.regionName, { month: 'long' });

      monthList.push({
        id: month + 1, // Month IDs are 1-based
        value: monthName
      });
    }

    return monthList;
  }

  public GetRegionName() {
    return this.regionName;
  }

  public GetYears() {
    const currentYear = new Date().getFullYear();
    const startYear = currentYear - 10;
    const endYear = currentYear;

    const yearList: any[] = [];

    for (let year = endYear; year >= startYear; year--) {
      yearList.push({
        id: year,
        value: year
      });
    }

    return yearList;
  }

  /*************************************
  * 2. Format Web API Response Data to local
  ************************************/

  // Format a date and time string
  public FormatDateTime(dateString: any) {
    const dateTime = new Date(dateString + 'Z');
    const formattedDate = dateTime.toLocaleString(this.regionName, { timeZone: this.timeZone });
    return formattedDate;
  }

  // Format a time string
  public FormatTime(timeString: any) {
    const [hours, minutes, seconds] = timeString.split(':');
    const currentDate = new Date();
    currentDate.setHours(hours, minutes, seconds);
    const localTime = currentDate.toLocaleTimeString(this.regionName, { timeZone: this.timeZone });
    return localTime;
  }

  // Format a date string
  public FormatDate(dateString: any): string {
    const dateTime = new Date(dateString + 'Z');

    // Format the date as a string without time
    const options: any = { timeZone: this.timeZone, year: 'numeric', month: '2-digit', day: '2-digit' };
    const formattedDate = dateTime.toLocaleDateString(this.regionName, options);

    return formattedDate;
  }

  public FormatPeriod(dateString: any, period: any) {
    const dateTime = new Date(dateString + 'Z');
    if (period == 'daily') {
      // Format for daily period
      const formattedMonthYear = dateTime.toLocaleDateString(this.regionName, { timeZone: this.timeZone, day: '2-digit', month: 'long', year: 'numeric' });
      return formattedMonthYear;
    } else if (period == 'monthly') {
      const formattedMonthYear = dateTime.toLocaleDateString(this.regionName, { timeZone: this.timeZone, month: 'long', year: 'numeric' });
      return formattedMonthYear;
    } else {
      const formattedYear = dateTime.toLocaleDateString(this.regionName, { timeZone: this.timeZone, year: 'numeric' });
      return formattedYear;
    }
  }

  public FormatPeriodHour(dateString: any): string {
    const dateTime = new Date(dateString + 'Z');
    const formattedHour = dateTime.toLocaleTimeString(this.regionName, {
      timeZone: this.timeZone,
      hour: 'numeric',
      minute: 'numeric',
      hour12: false
    });
    return formattedHour;
  }

  public FormatPeriodDay(dateString: any): string {
    const dateTime = new Date(dateString + 'Z');
    const formattedHour = dateTime.toLocaleDateString(this.regionName, { timeZone: this.timeZone });
    return formattedHour;
  }

  public FormatPeriodMonth(dateString: any): string {
    const dateTime = new Date(dateString + 'Z');
    const formattedMonthYear = dateTime.toLocaleDateString(this.regionName, { timeZone: this.timeZone, month: 'long', year: 'numeric' });
    return formattedMonthYear;
  }

  // Return formatted month and year from a date string
  public FormatMonthYear(dateString: any): string {
    const dateTime = new Date(dateString + 'Z');
    const formattedMonth = dateTime.toLocaleString(this.regionName, { timeZone: this.timeZone, month: 'long' });
    const year = dateTime.getFullYear();
    return `${formattedMonth} ${year}`;
  }

  // Format a number with decimal places
  public FormatNumber(number: any) {
    const currencyFormat = new Intl.NumberFormat(this.regionName, {
      style: 'decimal',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
    return currencyFormat.format(number);
  }

  // Format a number as a percentage
  public FormatPercent(number: any) {
    return `${this.FormatNumber(number)}%`;
  }

  // Format an amount as currency
  public FormatCurrency(amount: any) {
    const currencyFormat = new Intl.NumberFormat(this.regionName, {
      style: 'currency',
      currency: this.currencyName
    });
    return currencyFormat.format(amount);
  }

  // Get number formatting settings
  public GetNumberFormat() {
    const numberFormat = {
      symbol: this.currencySymbol,
      decimalSeparator: this.decimalSeparator,
      groupSeparator: this.groupSeparator,
      placeholder: `0${this.decimalSeparator}00`
    };
    return numberFormat;
  }

  /*************************************
  * 3. Initialize HTML Form Fields & Format HTML Form Field to APIs & APIs to HTML Form Field
  ************************************/

  //Initialize select picker (bootstrap-select)
  InitSummernote(element: string, code: any) {
    $(element).summernote('code', code);
  }

  GetSummernoteCode(element: string) {
    return $(element).summernote('code');
  }

  //Initialize tooltip (bootstrap)
  InitTooltip() {
    $(".splash").fadeOut("slow");
    $("[data-bs-toggle='tooltip']").tooltip('hide');
    $("[data-bs-toggle='tooltip']").tooltip();
  }

  // Initialize Cleave number input for formatting numeric values
  InitCleaveNumber(element: string): void {
    const numFormat = this.GetNumberFormat();
    document.querySelectorAll(element).forEach((el) => {
      new Cleave(el, {
        numeral: true,
        numeralDecimalMark: numFormat.decimalSeparator,
        delimiter: '',
      });
    });
  }

  InitPositiveCleaveNumber(element: string): void {
    const numFormat = this.GetNumberFormat();
    document.querySelectorAll(element).forEach((el) => {
      new Cleave(el, {
        numeral: true,
        numeralDecimalMark: numFormat.decimalSeparator,
        delimiter: '',
        numeralPositiveOnly: true
      });
    });
  }

  // Initialize Cleave number input for formatting numeric values
  InitIntCleaveNumber(element: string): void {
    document.querySelectorAll(element).forEach((el) => {
      new Cleave(el, {
        numeral: true,
        numeralDecimalMark: '',
        delimiter: '',
      });
    });
  }

  //Get Current Date for date field
  CurrentHTMLDate() {
    try {
      // Get the current date in the specified time zone
      const current = new Date();
      const options: any = { timeZone: this.timeZone, year: 'numeric', month: '2-digit', day: '2-digit' };
      const now = new Date(current.toLocaleDateString('en-US', options));

      // Format the date components
      const month = (now.getMonth() + 1).toString().padStart(2, '0');
      const day = now.getDate().toString().padStart(2, '0');

      // Construct and return the formatted date string
      return `${now.getFullYear()}-${month}-${day}`;
    } catch (error) {
      console.error('Error in CurrentHTMLDate:', error);
      return '';
    }
  }

  APIDateTimeToHTMLDate(utcDateTime: string) {
    // Ensure 'Z' is removed before parsing (if present)
    const dateTime = new Date(utcDateTime + 'Z');

    // Convert to the target time zone
    const options: any = { timeZone: this.timeZone, year: 'numeric', month: '2-digit', day: '2-digit' };
    const formattedDateString = dateTime.toLocaleString('en-US', options);

    // Extract year, month, and day components
    const parts = formattedDateString.split('/');
    const year = parts[2];
    const month = parts[0].padStart(2, '0');
    const day = parts[1].padStart(2, '0');

    // Format as "yyyy-MM-dd"
    const formattedResult = `${year}-${month}-${day}`;

    return formattedResult;
  }

  HtmlDateToAPIDateTime(date: string) {
    const dateRegex = /^\d{4}-\d{2}-\d{2}$/;
    if (!date.match(dateRegex)) {
      throw new Error('Invalid date format. Expected format: YYYY-MM-DD');
    }
    const [year, month, day] = date.split('-').map(Number);
    const inputDate = new Date(year, month - 1, day);
    const options: any = { timeZone: this.timeZone, hour: '2-digit', minute: '2-digit', hour12: false };
    let rawTime = inputDate.toLocaleTimeString('en-US', options);
    // Handle special case for "24:00:00"
    if (rawTime.startsWith('24:')) {
      rawTime = '00' + rawTime.substring(2);
    }
    const [hour, minute] = rawTime.split(':');
    const formattedDateTime = `${date}T${hour}:${minute}:00.000Z`;

    return formattedDateTime;
  }

  HTMLTimeToAPITime(time: any) {
    const [hours, minutes] = time.split(':');
    const apiTimeValue = `${hours}:${minutes}:00`;
    return apiTimeValue;
  }

  LocaleToAPINum(num: any): number {
    let str = String(num);
    const separator = this.GetNumberFormat().decimalSeparator;
    const regex = new RegExp(`[^0-9${separator}]`, 'g');
    const hasSeparator = str.includes(separator);
    const numericStr = hasSeparator ? str.replace(regex, '').replace(separator, '.') : str;
    return parseFloat(numericStr) || 0;
  }

  APINumToLocale(num: any): string {
    const separator = this.GetNumberFormat().decimalSeparator;
    const numStr = num.toString();
    return numStr.includes('.') ? numStr.replace('.', separator) : numStr;
  }

  public ConvertNgFormToFormData(form: NgForm): FormData {
    const formData = new FormData();

    Object.entries(form).forEach(([key, value]) => {
      if (value instanceof FileList) {
        for (let i = 0; i < value.length; i++) {
          formData.append(key, value[i]);
        }
      } else {
        formData.append(key, value);
      }
    });

    return formData;
  }

  public async LoadImages(attr: string) {
    const elements: any = document.querySelectorAll(attr);
    const cache = await caches.open('image-cache');

    for (const element of elements) {
      const imageUrl = String(element.getAttribute('data-url'));

      if (imageUrl) {
        try {
          const cachedResponse = await cache.match(imageUrl);

          if (cachedResponse) {
            const dataUrl = await cachedResponse.blob();
            this.UpdateImageSource(element, dataUrl);
          } else {
            const response: any = await this.http.get(imageUrl, { responseType: 'blob' }).toPromise();
            this.UpdateImageSource(element, response);
            cache.put(imageUrl, new Response(response, { status: 200, statusText: 'OK' }));
          }
        } catch (error) {
          this.SetDefaultImage(element);
        }
      } else {
        this.SetDefaultImage(element);
      }
    }
  }

  private UpdateImageSource(element: HTMLImageElement, dataUrl: Blob) {
    element.src = URL.createObjectURL(dataUrl);
  }

  private SetDefaultImage(element: HTMLImageElement) {
    element.src = '/assets/images/default.png';
  }

  // Handle API errors
  public HandleApiError(error: any): void {
    if (error.status === 401) {
      this.Logout();
    } else if (error.status === 403) {
      this.ErrorMessage(this.Localize('Error!'), this.Localize('Access is denied.'));
    } else if (error.status === 404) {
      this.ErrorMessage(this.Localize('Error!'), this.Localize('Not Found.'));
    } else if (error.status === 0) {
      this.NoInternetMessage(
        this.Localize('No Internet Connection!'),
        this.Localize('Make sure that Wi-Fi or mobile data is turned on, then try again.')
      );
    } else {
      const errorMsg = error.error?.message || error.statusText;
      this.ErrorMessage(error.status, errorMsg);
    }
  }

  // Toaster Messages
  public SuccessMessage(title: string, description: string) {
    let msg = `<div class="toast-header bg-primary text-on-primary">
    <i class="fa fa-check me-2"></i> <strong class="me-auto">${title}</strong>
    <button type="button" class="btn text-on-primary m-0 p-0" data-bs-dismiss="toast"><i class="fa fa-x"></i></button>
    </div>
    <div class="toast-body">${description}</div>`;
    let toasterSection = document.getElementById('toasters') as HTMLElement;
    let toaster = document.createElement('div');
    toaster.classList.add("toast", "fade", "show");
    toaster.innerHTML = msg;

    toasterSection.prepend(toaster);

    setTimeout(() => { toaster.remove(); }, 30000);
  }

  public ErrorMessage(title: string, description: string) {
    let msg = `<div class="toast-header bg-danger text-white">
    <i class="fa fa-triangle-exclamation me-2"></i> <strong class="me-auto">${title}</strong>
    <button type="button" class="btn text-white m-0 p-0" data-bs-dismiss="toast"><i class="fa fa-x"></i></button>
    </div>
    <div class="toast-body">${description}</div>`;
    let toasterSection = document.getElementById('toasters') as HTMLElement;
    let toaster = document.createElement('div');
    toaster.classList.add("toast", "fade", "show");
    toaster.innerHTML = msg;

    toasterSection.prepend(toaster);

    setTimeout(() => { toaster.remove(); }, 30000);
  }

  public InfoMessage(title: string, description: string) {
    let msg = `<div class="toast-header bg-info text-white">
    <i class="fa fa-triangle-exclamation me-2"></i> <strong class="me-auto">${title}</strong>
    <button type="button" class="btn text-white m-0 p-0" data-bs-dismiss="toast"><i class="fa fa-x"></i></button>
    </div>
    <div class="toast-body">${description}</div>`;
    let toasterSection = document.getElementById('toasters') as HTMLElement;
    let toaster = document.createElement('div');
    toaster.classList.add("toast", "fade", "show");
    toaster.innerHTML = msg;

    toasterSection.prepend(toaster);

    setTimeout(() => { toaster.remove(); }, 30000);
  }

  public WarningMessage(title: string, description: string) {
    let msg = `<div class="toast-header bg-warning text-white">
    <i class="fa fa-triangle-exclamation me-2"></i> <strong class="me-auto">${title}</strong>
    <button type="button" class="btn text-white m-0 p-0" data-bs-dismiss="toast"><i class="fa fa-x"></i></button>
    </div>
    <div class="toast-body">${description}</div>`;
    let toasterSection = document.getElementById('toasters') as HTMLElement;
    let toaster = document.createElement('div');
    toaster.classList.add("toast", "fade", "show");
    toaster.innerHTML = msg;

    toasterSection.prepend(toaster);

    setTimeout(() => { toaster.remove(); }, 30000);
  }

  public NoInternetMessage(title: string, description: string) {
    let msg = `<div class="toast-header bg-dark text-white">
    <i class="fa fa-triangle-exclamation me-2"></i> <strong class="me-auto">${title}</strong>
    <button type="button" class="btn text-white m-0 p-0" data-bs-dismiss="toast"><i class="fa fa-x"></i></button>
    </div>
    <div class="toast-body">${description}</div>`;
    let toasterSection = document.getElementById('toasters') as HTMLElement;
    let toaster = document.createElement('div');
    toaster.classList.add("toast", "fade", "show");
    toaster.innerHTML = msg;

    toasterSection.prepend(toaster);

    setTimeout(() => { toaster.remove(); }, 30000);
  }

  /*
  Theme 
  */
  public SwitchNightMode() {
    let thememode = localStorage.getItem("news_portal_Thememode");
    if (thememode == 'dark') {
      localStorage.setItem("news_portal_Thememode", 'light');
      $('body').addClass('light');
      $('body').removeClass('dark');
    }
    else {
      localStorage.setItem("news_portal_Thememode", 'dark');
      $('body').addClass('dark');
      $('body').removeClass('light');
    }
  }

  public ApplyNightMode() {
    let thememode = localStorage.getItem("news_portal_Thememode");
    if (thememode == 'dark') {
      $('body').addClass('dark');
      $('body').removeClass('light');
    }
    else {
      $('body').addClass('light');
      $('body').removeClass('dark');
    }
  }

  public POSPageInit() {
    setTimeout(() => {
      var hide_dropdown = function () {
        let selected1 = $(".sidebar-layout .app-menu .menu li.has-submenu");
        if (selected1.length) {
          selected1.find('.submenu').removeAttr('style');
        }
      };

      let w = $(window);
      if (w.outerWidth() > 1024) {
        if ($('.sidebar-layout').length) {
          $('.sidebar-layout').addClass('pos-page');
          $('.sidebar-layout').addClass('sidebar-mini');
          hide_dropdown();
        }
      }
    }, 500);
  }

  public POSPageDestroy() {
    let w = $(window);
    if (w.outerWidth() > 1024) {
      if ($('.sidebar-layout').length) {
        $('.sidebar-layout').removeClass('pos-page');
      }
    }
  }
  private checkRouteExists(routes: Route[], returnUrl: string): boolean {
    for (let route of routes) {
      if (route.path === returnUrl) {
        return true;
      }
      if (route.children) {
        if (this.checkRouteExists(route.children, returnUrl)) {
          return true;
        }
      }
    }
    return false;
  }

  private removeLeadingSlash(url: string): string {
    return url.startsWith('/') ? url.substring(1) : url;
  }
}
