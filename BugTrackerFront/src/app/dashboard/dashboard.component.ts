import { Component, Inject, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { IAuthService } from '../auth/Iauth-service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})

export class DashboardComponent implements OnInit {
  // Does the user have Dev role?
  isDev: boolean = false;

  constructor(public authService: IAuthService) {
    this.authService.isDev
      .subscribe(result => {
        this.isDev = result;
      });
  }

  ngOnInit(): void {
    this.isDev = this.authService.isDevUser();
  }
     

}
