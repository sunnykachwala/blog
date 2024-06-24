import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../interceptors/auth.guard';
import { BlogListComponent } from './bloglist/bloglist.component';

const routes: Routes = [
  { path: 'list', component: BlogListComponent, canActivate: [AuthGuard], data: { permission: "Blog.List" } },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
