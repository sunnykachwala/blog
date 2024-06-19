import { Component } from '@angular/core';
import { AppService } from '../services/app.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  constructor(   
    public app: AppService,
  ) { }
  ngOnInit(): void {
    this.app.InitTooltip();
  }
}
