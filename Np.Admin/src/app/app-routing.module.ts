import { NgModule } from '@angular/core';
import { Routes, RouterModule, CanActivate } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { AuthGuard } from './interceptors/auth.guard';

import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { HomeMenuComponent } from './home-menu/home-menu.component';


const routes: Routes = [

  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'auth/login', component: LoginComponent, canActivate: [AuthGuard], data: { permission: "Login" } },

  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard], data: { permission: "Dashboard" } },
  
  { path: 'homemenu', component: HomeMenuComponent, canActivate: [AuthGuard],data: { permission: "Home" } },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
