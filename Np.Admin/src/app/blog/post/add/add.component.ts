import {  ChangeDetectorRef, Component,  OnInit } from '@angular/core';
import { AppService } from '@apps/services/app.service';
import { FormBuilder, FormGroup, Validators ,UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { PostService } from '@apps/blog/services/post.service';
import { first } from 'rxjs';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})
export class PostAddComponent  implements OnInit{
  articleForm: FormGroup;
  apiMessage:string  ='';
  fileList: NzUploadFile[] =[] ;
  summernoteConfig: any = {
    placeholder: 'Write your content here...',
    tabsize: 2,
    height: 200
  };

  constructor(public app: AppService,
    private fb: FormBuilder, 
    private router: Router,
  private postService:PostService,
  private cdr: ChangeDetectorRef) {

    this.articleForm = this.fb.group({
      title: ['', [Validators.required]],
      content: ['', []],
      defaultImage: [''],
      isPublished: [true],
      dispalyOrder: [1],
      selectedCategories: [[]],
      selectedTags: [[]],
      keywords: [''],
      metaDescription: [''],
      metaTitle: [''],
      slug: ['']
    });
    app.InitTooltip();
  }
  // constructor(public app: AppService,   
  //   private fb: UntypedFormBuilder,
  //   private router: Router,) {
  //   app.InitTooltip();
    
  // }
 

  ngOnInit(): void {
    setTimeout(() => {  this.app.InitSummernote('.summernote', '') ; }, 1000);   
  }
  onCancel() {
    // Handle the cancel action here
    this.articleForm.reset();
  }
  backToListing(e: MouseEvent): void {
    e.preventDefault();
    this.router.navigate(['/blog/post']);
  }
  onSave() {
    const content = this.app.GetSummernoteCode('.summernote');
    console.log('content', content);

    if (this.fileList.length > 1) {
      //this.message.error("You can upload one file at a time.");
      return;
    }

    switch (this.fileList[0].type) {
      case "image/jpeg":
      case "image/png":
    //  case "application/pdf":

        break;

      default:
       // this.message.error("Please choose an image or PDF file.");
        return;

        break;
    }
    var postData= {
       
      title: this.articleForm.value.title,
      content: content,
      defaultImage :this.fileList[0],
      isPublished: this.articleForm.value.isPublished,
      dispalyOrder: this.articleForm.value.dispalyOrder,
      selectedCategories: this.articleForm.value.selectedCategories,
      selectedTags: this.articleForm.value.selectedTags,
      keywords: this.articleForm.value.keywords,
      metaDescription: this.articleForm.value.metaDescription,
      metaTitle: this.articleForm.value.metaTitle,
      slug: this.articleForm.value.slug,
    };
    const sourceDocument:any = this.fileList[0];

    if (this.articleForm.valid) {   
      const formData = new FormData();
      formData.append('SourceFile', sourceDocument, sourceDocument.name);
      formData.append('title', postData.title);
      formData.append('content', postData.content);
      formData.append('isPublished', postData.isPublished);
      formData.append('dispalyOrder', postData.dispalyOrder);
      formData.append('selectedCategories', postData.selectedCategories);
      formData.append('selectedTags', postData.selectedTags);
      formData.append('titkeywordsle', postData.keywords);
      formData.append('metaDescription', postData.metaDescription);
      formData.append('metaTitle', postData.metaTitle);
      formData.append('slug', postData.slug);
      this.postService.add(formData)
        .pipe(first())
        .subscribe({
          next: (response: any) => {
            this.apiMessage = response.message;
            this.cdr.markForCheck();
            setTimeout(() => {  this.app.InitSummernote('.summernote', '') ;this.articleForm.reset(); }, 1000);   
            
 
          },
          error: (error: any) => {
           
          }
        })
        .add(() => {
          
        });

    }
  }
  beforeUpload = (file: NzUploadFile): boolean => {
    this.fileList = this.fileList.concat(file);
    return false;
  };
  handleChange(info: { file: NzUploadFile }): void {
    if (info.file.status === 'done') {
      this.articleForm.patchValue({ defaultImage: info.file.response.url });
    }
  }
}
