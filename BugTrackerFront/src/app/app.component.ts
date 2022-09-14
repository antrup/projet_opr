import { Component, Inject, OnInit } from '@angular/core';
import { IAuthService } from './auth/Iauth-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Bug Tracker';

  constructor(@Inject(IAuthService) private authService: IAuthService) { }
  ngOnInit(): void {
    this.authService.init();
  }
}
