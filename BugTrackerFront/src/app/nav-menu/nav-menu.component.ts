import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IAuthService } from '../auth/Iauth-service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {

  isLoggedIn: boolean = false;
  isAdmin: boolean = false;

  // Subscribe to user status observables
  constructor(public authService: IAuthService,
    private router: Router) {
    this.authService.authStatus
      .subscribe(result => {
        this.isLoggedIn = result;
      });
    this.authService.isAdmin
      .subscribe(result => {
        this.isAdmin = result;
      });
  }

  // Method binded to logout button
  onLogout(): void {
    this.authService.logout();
    this.router.navigate(["/login"]);
  }

  // Get user status on init
  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated();
    this.isAdmin = this.authService.isAdminUser();
  }


}
