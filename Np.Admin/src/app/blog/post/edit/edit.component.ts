import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AppService } from '@apps/services/app.service';
import { FormBuilder, FormGroup, Validators, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { PostService } from '@apps/blog/services/post.service';
import { first } from 'rxjs';
import { CategoryService } from '@apps/blog/services/category.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class PostEditComponent implements OnInit {
  articleForm: FormGroup;
  apiMessage: string = '';
  fileList: NzUploadFile[] = [];
  categoryList:[] =[];
  articleId:string='';
  summernoteConfig: any = {
    placeholder: 'Write your content here...',
    tabsize: 2,
    height: 200
  };


  categoryOptions = [{ value: '3fa85f64-5717-4562-b3fc-2c963f66afa6', label: 'Category 1' }, { value: '3fa85f64-5717-4562-b3fc-2c963f66afa7', label: 'Category 2' }]
  tagOptions = [{ value: '3fa85f64-5717-4562-b3fc-2c963f66afa6', label: 'Tag 1' }, { value: '3fa85f64-5717-4562-b3fc-2c963f66afa7', label: 'Tag 2' }]

  constructor(public app: AppService,
    private fb: FormBuilder,
    private router: Router,
    private postService: PostService,
    private cdr: ChangeDetectorRef,
    private categoryService: CategoryService,
    private route: ActivatedRoute,) {

    this.articleForm = this.fb.group({
      title: ['', [Validators.required]],
      content: ['', []],
      defaultImage: [''],
      isPublished: [true],
      displayOrder: [1],
      selectedCategories: [[]],
      selectedTags: [[]],
      keywords: [''],
      metaDescription: [''],
      metaTitle: [''],
      slug: ['']
    });
    app.InitTooltip();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(queryParams => {
      this.articleId = queryParams['id'] ?? '53deb952-4821-42ed-bc39-727ff0c6f9b0';
    });
    setTimeout(() => { this.app.InitSummernote('.summernote', ''); }, 1000);
    let filter  = {
      page :1,
      pageSize:25,
      search :'',
      isActive:true
    };
    this.getCategories(filter);
    setTimeout(() => {
      this.cdr.markForCheck();
    }, 500);
    this.getPost(this.articleId);
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
 
    if (this.fileList.length > 1) {
      //this.message.error("You can upload one file at a time.");
      return;
    }

    switch (this.fileList[0].type) {
      case "image/jpeg":
      case "image/png":
        break;
      default:
        // this.message.error("Please choose an image or PDF file.");
        return;
    }
    var postData = {
      title: this.articleForm.value.title,
      content: content,
      defaultImage: this.fileList[0],
      isPublished: this.articleForm.value.isPublished,
      displayOrder: this.articleForm.value.displayOrder,
      selectedCategories: this.articleForm.value.selectedCategories,
      selectedTags: this.articleForm.value.selectedTags,
      keywords: this.articleForm.value.keywords,
      metaDescription: this.articleForm.value.metaDescription,
      metaTitle: this.articleForm.value.metaTitle,
      slug: this.articleForm.value.slug,
    };
    const sourceDocument: any = this.fileList[0];

    if (this.articleForm.valid) {
      const formData = new FormData();
      formData.append('SourceFile', sourceDocument, sourceDocument.name);
      formData.append('title', postData.title);
      formData.append('content', postData.content);
      formData.append('isPublished', postData.isPublished);
      formData.append('displayOrder', postData.displayOrder);
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
            this.app.SuccessMessage(this.app.Localize("Success!"), this.app.Localize("You have been successfully added blog"));
            setTimeout(() => { this.app.InitSummernote('.summernote', ''); this.articleForm.reset(); }, 1000);


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

  getCategories(filter:any){ 
    this.categoryService.get(filter)
    .pipe(first())
    .subscribe({
      next: (response: any) => {
        this.categoryList = response.map((category: any) => {
          return {
            value : category.id,
            label : category.title,
          };
        });;
        this.cdr.markForCheck();
 
      },
      error: (error: any) => {
      }
    })
    .add(() => {
      
    });    
  }

  getPost(id:any){ 
    this.postService.getById(id)
    .pipe(first())
    .subscribe({
      next: (response: any) => {
        setTimeout(() => { this.app.InitSummernote('.summernote', response.content); }, 1000); 
        this.articleForm.get("title")!.setValue(response.title);       
        this.articleForm.get("isPublished")!.setValue(response.isPublished);
        this.articleForm.get("displayOrder")!.setValue(response.displayOrder);
        this.articleForm.get("keywords")!.setValue(response.keywords);
        this.articleForm.get("metaDescription")!.setValue(response.metaDescription);
        this.articleForm.get("metaTitle")!.setValue(response.metaTitle);
        this.articleForm.get("slug")!.setValue(response.slug); 
        this.cdr.markForCheck(); 
      },
      error: (error: any) => {
      }
    })
    .add(() => {
      
    });    
  }

  onInput(event: Event): void {

    let filter  = {
      page :1,
      pageSize:25,
      search :'',
      isActive:true
    };
    const value = (event.target as HTMLInputElement).value;
    if(value && value.length >3){
      filter.search =value;
      this.getCategories(filter);
    }
    console.log(value);
    //this.categoryList = value ? [value, value + value, value + value + value] : [];
  }
}
