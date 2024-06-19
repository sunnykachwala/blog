import { Component, OnInit } from '@angular/core';
import { AppService } from '../services/app.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  constructor(public app: AppService) {
    app.Redirect();
    app.InitTooltip();
  }

  ngOnInit() {
  }
}
