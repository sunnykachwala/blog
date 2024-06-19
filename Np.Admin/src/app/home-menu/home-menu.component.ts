import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AppService } from '../services/app.service';

@Component({
  selector: 'app-home-menu',
  templateUrl: './home-menu.component.html',
  styleUrls: ['./home-menu.component.css']
})
export class HomeMenuComponent {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public app: AppService,
  ) { }

  ngOnInit(): void {
    this.app.InitTooltip();
  }

}
