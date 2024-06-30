import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']

})


export class ListComponent implements OnInit {
  studyData: any[] = [];
  searchData: any | null = null;
  currentPage = 1;
  serchFilters!: FormGroup;
  pageSize: number = 25;
  public hasPageRight: boolean = false;
  public hasAddRight: boolean = false;
  public hasEditRight: boolean = false;
  public hashDeleteRight: boolean = false;
  public isLoading = false;
  constructor(public app: AppService) {
    app.InitTooltip();
  }

  ngOnInit(): void {
  }

}
