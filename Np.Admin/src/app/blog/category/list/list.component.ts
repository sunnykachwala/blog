import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AppService } from '../../../services/app.service';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class CategoryListComponent implements OnInit {
  studyData: any[] = [];
  searchData: any | null = null;
  currentPage = 1;
  serchFilters!: FormGroup;

  public hasPageRight: boolean = false;
  public hasAddRight: boolean = false;
  public hasEditRight: boolean = false;
  public hashDeleteRight: boolean = false;
  public isLoading = false;

  total = 1;
  listOfPost: any[] = [];
  loading = true;
  pageSize: number = 25;
  pageIndex :number = 1;
  filterGender = [
    { text: 'male', value: 'male' },
    { text: 'female', value: 'female' }
  ];
  constructor(public app: AppService,    private router: Router,
    private route: ActivatedRoute) {
    app.InitTooltip();
  }

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
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
    this.loading = true;
    // this.randomUserService.getUsers(pageIndex, pageSize, sortField, sortOrder, filter).subscribe(data => {
    //   this.loading = false;
    //   this.total = 200; // mock the total data here
    //   this.listOfRandomUser = data.results;
    // });
  }

  addCategory(){
    this.router.navigate(["/blog/category/add"]);
  }  
}
