import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CategoryService } from '@apps/blog/services/category.service';
import { AppService } from '@apps/services/app.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { first } from 'rxjs';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})

export class AddCategoryComponent implements OnInit {
  categoryForm: FormGroup;
  selectedFile: File | null = null;
  apiMessage:string  ='';
  categoryList: [] =[];
  options: string[] = [];
  inputValue: string ='';

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private message: NzMessageService,
    public app: AppService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.categoryForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(120)]],
      details: ['', [Validators.required, Validators.maxLength(300)]],
      parentCategoryId: [null],
      isActive: [true, [Validators.required]],
      displayOrder: [1],
      keywords: ['', [Validators.maxLength(300)]],
      metaDescription: ['', [Validators.maxLength(300)]],
      metaTitle: ['', [Validators.maxLength(160)]],
      slug: ['', [Validators.required, Validators.maxLength(160)]]
    });
  }
  ngOnInit(): void {
   this.getCategories();
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    }
  }

  onSubmit() {
    if (this.categoryForm.valid) {
      const formData = new FormData();
      formData.append('Title', this.categoryForm.get('title')?.value);
      formData.append('Details', this.categoryForm.get('details')?.value);
      if(this.categoryForm.get('parentCategoryId')?.value == null){
        formData.append('ParentCategoryId', '');
      }else{
        formData.append('ParentCategoryId', this.categoryForm.get('parentCategoryId')?.value);
      }
      formData.append('IsActive', this.categoryForm.get('isActive')?.value);
      formData.append('DisplayOrder', this.categoryForm.get('displayOrder')?.value);
      formData.append('Keywords', this.categoryForm.get('keywords')?.value);
      formData.append('MetaDescription', this.categoryForm.get('metaDescription')?.value);
      formData.append('MetaTitle', this.categoryForm.get('metaTitle')?.value);
      formData.append('Slug', this.categoryForm.get('slug')?.value);
      
      if (this.selectedFile) {
        formData.append('SourceFile', this.selectedFile);
      }
      console.log('Form Submitted!', this.categoryForm.value);
      // You can handle the form submission here
      this.categoryService.add(formData)
      .pipe(first())
      .subscribe({
        next: (response: any) => {
          this.apiMessage = response.message;
          this.cdr.markForCheck();
          this.message.success('Category created successfully!');
          this.categoryForm.reset();
          this.selectedFile = null;
          

        },
        error: (error: any) => {
          this.message.error('Failed to create category!');
        }
      })
      .add(() => {
        
      });    
    }
  }

  getCategories(){
    let filter  = {
      page :1,
      pageSize:25,
      search :'',
      isActive:true
    };

    this.categoryService.get(filter)
    .pipe(first())
    .subscribe({
      next: (response: any) => {
        this.apiMessage = response.message;
        this.cdr.markForCheck();
        this.message.success('Category created successfully!');
        this.categoryForm.reset();
        this.selectedFile = null;
        

      },
      error: (error: any) => {
        this.message.error('Failed to create category!');
      }
    })
    .add(() => {
      
    });    
  }

  onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    console.log(value);
    this.options = value ? [value, value + value, value + value + value] : [];
  }
}
