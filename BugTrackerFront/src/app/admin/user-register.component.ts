import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../base-form.component';
import { Result } from './interfaces/result';
import { UserRegistration } from './interfaces/userRegistration';
import { UserResgitrationService } from './user-resgistration.service';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
  providers: [UserResgitrationService]
})
export class UserRegisterComponent extends BaseFormComponent implements OnInit {

  result!: Result;

  // Router and UserResgitrationService injection
  constructor(
    private userResgistrationService: UserResgitrationService,
    private router: Router)
  {
    super()
  }

  ngOnInit(): void {
    // Initialise form fields and associated validators
    this.form = new FormGroup({
      username: new FormControl('', [Validators.required,
      Validators.minLength(4),
      Validators.maxLength(10),
        Validators.pattern("^[a-zA-Z 0-9 -_]+$")]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required,
        Validators.minLength(7),
        Validators.maxLength(15),
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@$!%*#?&])[a-zA-Z0-9@$!%*#?&]+$')]),
      roleDev: new FormControl(''),
      roleAdmin: new FormControl(''),
    });
  }

  onSubmit() {
    let user: UserRegistration = {
      username: this.form.controls["username"].value,
      email: this.form.controls["email"].value,
      password: this.form.controls["password"].value,
      roledev: (this.form.controls["roleDev"].value === true),
      roleadmin: (this.form.controls["roleAdmin"].value === true)
    };

    this.userResgistrationService
      .post(user)
      .subscribe(result => {
        this.result = result;
        if (result.success)
          this.router.navigate(['/admin']);
      }, error => console.error(error));
  }
}

