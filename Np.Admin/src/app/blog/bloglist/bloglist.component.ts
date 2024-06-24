import { Component } from '@angular/core';
import { AppService } from '../../services/app.service';

@Component({
  selector: 'app-bloglist',
  templateUrl: './bloglist.component.html',
  styleUrls: ['./bloglist.component.css']
})
export class BlogListComponent {
  constructor(public app: AppService) {
    app.InitTooltip();
  }
}
