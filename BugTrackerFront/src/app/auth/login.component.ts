import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseFormComponent } from '../base-form.component';
import { LoginRequest } from './interfaces/login-request';
import { LoginResult } from './interfaces/login-result';
import { Router } from '@angular/router';
import { IAuthService } from './Iauth-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent extends BaseFormComponent implements OnInit {

  title?: string;
  loginResult?: LoginResult;

  // Router and AuthService injection
  constructor(
    private router: Router,
    private authService: IAuthService) {
    super();
  }
  ngOnInit() {
    // Initialise form fields and associated validators
    this.form = new FormGroup({
      username: new FormControl('', [Validators.required,
        Validators.minLength(4),
        Validators.maxLength(10),
        Validators.pattern('^[A-Za-z0-9\-_]+$')]),
      password: new FormControl('', [Validators.required,
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@$!%*#?&])[a-zA-Z0-9@$!%*#?&]+$')])
    });
  }

  onSubmit() {
    var loginRequest = <LoginRequest>{};
    loginRequest.username = this.form.controls['username'].value;
    loginRequest.password = this.form.controls['password'].value;
    this.authService
      .login(loginRequest)
      .subscribe(result => {
        this.loginResult = result;
        if (result.success) {
          this.router.navigate(["/"]);
        }
      }, error => {
        console.log(error);
        if (error.status == 401) {
          this.loginResult = error.error;
        }
      });

  }
}

