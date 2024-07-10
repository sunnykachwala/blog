import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { LoginComponent } from './auth/login/login.component';

import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { IconsProviderModule } from './icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NgZorroAntdModule } from './ng-zorro/ng-zorro-antd.module';
import { AppService } from './services/app.service';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { AuthenticationService } from './services/authentication.service';
import { appInitializer } from './services/app.initializer';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeMenuComponent } from './home-menu/home-menu.component';
import { MyProfileComponent } from './myaccount/myprofile/myprofile.component';
import { AppSettingComponent } from './settings/appsetting/appsetting.component';
import { RolesComponent } from './settings/roles/roles.component';
import { TemplatesComponent } from './settings/templates/templates.component';

import { NgxSummernoteModule } from 'ngx-summernote';


//Material
import { NgSelectModule } from '@ng-select/ng-select';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    DashboardComponent,
    HomeMenuComponent,
    MyProfileComponent,
    AppSettingComponent,
    RolesComponent,
    TemplatesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NgZorroAntdModule,
    FormsModule,
    IconsProviderModule,
    NzLayoutModule,
    NzMenuModule,
    NgSelectModule,
    NgxSummernoteModule
    ],
  providers: [
    { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthenticationService] },
    AppService, { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
/*    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },*/
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent]
})
export class AppModule { }
