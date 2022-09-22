import { Component, OnInit } from '@angular/core';
import { IAuthService } from '../../shared/auth/Iauth-service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
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
