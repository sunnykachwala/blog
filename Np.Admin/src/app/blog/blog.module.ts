import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogRoutingModule } from './blog-routing.module';
import { BlogListComponent } from './bloglist/bloglist.component'


@NgModule({
  declarations: [
    BlogListComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule
  ],
  exports: [
    BlogListComponent,    
  ]
})
export class BlogModule { }
