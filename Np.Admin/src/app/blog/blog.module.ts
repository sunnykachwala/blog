import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogRoutingModule } from './blog-routing.module';
import { NgZorroAntdModule } from '../ng-zorro/ng-zorro-antd.module';

import { ListComponent } from './post/list/list.component';
import { PostAddComponent } from './post/add/add.component';
import { EditComponent } from './post/edit/edit.component';
import { CategoryListComponent } from './category/list/list.component';
import { TagListComponent } from './tag/list/list.component';
import { PageListComponent } from './page/list/list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxSummernoteModule } from 'ngx-summernote';




@NgModule({
  declarations: [
    ListComponent,
    PostAddComponent,
    EditComponent,
    CategoryListComponent,
    TagListComponent,
    PageListComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    NgZorroAntdModule,
    ReactiveFormsModule,
    NgxSummernoteModule
  ],
  exports: [
    ListComponent,
    CategoryListComponent,
    TagListComponent
  ]
})
export class BlogModule { }
