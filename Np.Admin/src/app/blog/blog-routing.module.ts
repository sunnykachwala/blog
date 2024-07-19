import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../interceptors/auth.guard';
import { PostListComponent } from './post/list/list.component';
import { CategoryListComponent } from './category/list/list.component';
import { TagListComponent } from './tag/list/list.component';
import { PageListComponent } from './page/list/list.component';
import { PostAddComponent } from './post/add/add.component';
import { AddCategoryComponent } from './category/add/add.component';
import { PostEditComponent } from './post/edit/edit.component';


const routes: Routes = [
  { path: 'post', component: PostListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Post" } },
  { path: 'post/add', component: PostAddComponent, canActivate: [AuthGuard], data: { permission: "Blog.Post" } },
  { path: 'post/edit', component: PostEditComponent, canActivate: [AuthGuard], data: { permission: "Blog.Post" } },

  { path: 'category', component: CategoryListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Category" } },
  { path: 'category/add', component: AddCategoryComponent, canActivate: [AuthGuard], data: { permission: "Blog.Category" } },
  { path: 'category/edit', component: CategoryListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Category" } },

  { path: 'tag', component: TagListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Tag" } },
  { path: 'tag/add', component: TagListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Tag" } },
  { path: 'tag/edit', component: TagListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Tag" } },

  { path: 'page', component: PageListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Page" } },
  { path: 'page/add', component: PageListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Page" } },
  { path: 'page/edit', component: PageListComponent, canActivate: [AuthGuard], data: { permission: "Blog.Page" } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
