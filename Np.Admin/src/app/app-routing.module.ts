import { NgModule } from '@angular/core';
import { Routes, RouterModule, CanActivate } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { AuthGuard } from './interceptors/auth.guard';

import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { HomeMenuComponent } from './home-menu/home-menu.component';

//Settings Components
import { AppSettingComponent } from './settings/appsetting/appsetting.component';
import { RolesComponent } from './settings/roles/roles.component';
import { TemplatesComponent } from './settings/templates/templates.component';


//My Account
import { MyProfileComponent } from './myaccount/myprofile/myprofile.component';
const moduleRoutes: Routes = [
  { path: '', loadChildren: () => import('./blog/blog.module').then(m => m.BlogModule), canActivate: [AuthGuard] },
]

const routes: Routes = [

  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'auth/login', component: LoginComponent, canActivate: [AuthGuard], data: { permission: "Login" } },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard], data: { permission: "Dashboard" } },
  { path: 'homemenu', component: HomeMenuComponent, canActivate: [AuthGuard], data: { permission: "Home" } },

  { path: 'settings/appsetting', component: AppSettingComponent, canActivate: [AuthGuard], data: { permission: "Settings.AppSetting" } },
  { path: 'settings/roles', component: RolesComponent, canActivate: [AuthGuard], data: { permission: "Settings.Roles" } },
  { path: 'settings/templates', component: TemplatesComponent, canActivate: [AuthGuard], data: { permission: "Settings.Templates" } },
  { path: 'myaccount/myprofile', component: MyProfileComponent, canActivate: [AuthGuard], data: { permission: "MyProfile" } },
  { path: 'blog', loadChildren: () => import('./blog/blog.module').then(m => m.BlogModule) }, // Lazy-loaded blog module
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
