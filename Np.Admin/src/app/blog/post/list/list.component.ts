import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AppService } from '../../../services/app.service';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { Router, ActivatedRoute } from '@angular/router';
import { PostService } from '@apps/blog/services/post.service';
import { first } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { CategoryService } from '@apps/blog/services/category.service';
import { Articles } from '@apps/types/articel';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']

})

export class PostListComponent implements OnInit {
  searchData: any | null = null;
  currentPage = 1;
  searchFilters!: FormGroup;
  authorOptions = [{ value: '', label: 'author 1' }, { value: '', label: 'author 2' }]
  categoryList :[] =[];
  tagOption:[] =[];
  public hasPageRight: boolean = false;
  public hasAddRight: boolean = false;
  public hasEditRight: boolean = false;
  public hashDeleteRight: boolean = false;
  public isLoading = false;

  total = 1;
  listOfPost: Articles[] = [];
  loading = true;
  pageSize: number = 25;
  pageIndex: number = 1;
  filterGender = [
    { text: 'male', value: 'male' },
    { text: 'female', value: 'female' }
  ];
  constructor(public app: AppService, 
    private router: Router,
    private route: ActivatedRoute, 
    public fb: FormBuilder,
  private articleService:PostService, 
  private cdr: ChangeDetectorRef,
  private message: NzMessageService,
  private categoryService: CategoryService) {
    app.InitTooltip();
    this.searchFilters = this.fb.group({
      searchText: [''],
      authorSearch: ['']
    })
  }

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
   let filter  = {
    page :1,
    pageSize:25,
    search :'',
    isActive:true
  };
  this.getCategories(filter);
  }
  onQueryParamsChange(params: NzTableQueryParams): void {
    console.log(params);
    const { pageSize, pageIndex, sort, filter } = params;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
    this.loadDataFromServer(pageIndex, pageSize, sortField, sortOrder, filter);
  }


  loadDataFromServer(
    pageIndex: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filter: Array<{ key: string; value: string[] }>
  ): void {
    this.loading = false;
    this.isLoading = false
    // this.randomUserService.getUsers(pageIndex, pageSize, sortField, sortOrder, filter).subscribe(data => {
    //   this.loading = false;
    //   this.total = 200; // mock the total data here
    //   this.listOfRandomUser = data.results;
    // });

    let filterData  = {
      page :pageIndex,
      pageSize:pageSize,
      search :'',
      isActive:true,
      sortField: sortField,
      sortOrder:sortOrder,
      filters :filter
    };
    this.getPosts(filterData)
  }

  addPost() {
    this.router.navigate(["/blog/post/add"]);
  }

  getPosts(filter: any) {
    //this.isLoading=true;
    this.articleService.get(filter)
      .pipe(first())
      .subscribe({
        next: (response: any) => {      
         
         this.listOfPost = response;
          this.cdr.markForCheck();
          console.log(this.listOfPost );
          this.isLoading =false;
        },
        error: (error: any) => {
          this.message.error('Failed to retrieve post!');
        }
      })
      .add(() => {
        this.loading = false;
      });
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
        });
        this.cdr.markForCheck();
 
      },
      error: (error: any) => {
      }
    })
    .add(() => {
      
    });    
  }
}
