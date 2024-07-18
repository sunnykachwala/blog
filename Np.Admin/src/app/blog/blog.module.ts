import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogRoutingModule } from './blog-routing.module';
import { NgZorroAntdModule } from '../ng-zorro/ng-zorro-antd.module';

import { PostListComponent } from './post/list/list.component';
import { PostAddComponent } from './post/add/add.component';
import { EditComponent } from './post/edit/edit.component';

import { TagListComponent } from './tag/list/list.component';
import { PageListComponent } from './page/list/list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxSummernoteModule } from 'ngx-summernote';

import { CategoryListComponent } from './category/list/list.component';
import { AddCategoryComponent } from './category/add/add.component';




@NgModule({
  declarations: [
    PostListComponent,
    PostAddComponent,
    EditComponent,

    TagListComponent,
    PageListComponent,

    CategoryListComponent,
    AddCategoryComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    NgZorroAntdModule,
    ReactiveFormsModule,
    NgxSummernoteModule
  ],
  exports: [
    PostListComponent,   
    TagListComponent,

    CategoryListComponent,
    AddCategoryComponent
  ]
})
export class BlogModule { }
