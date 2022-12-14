import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../../shared/base-form.component';
import { DATAPATH, DataService } from '../../shared/data.service';
import { IDataService } from '../../shared/Idata-service';
import { UserInfo } from '../../shared/interfaces/userinfo';
import { Result } from './interfaces/result';
import { UserRegistration } from './interfaces/userRegistration';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
  providers: [{
    provide: IDataService,
    useClass: DataService<UserInfo, string, UserRegistration>,
  },
  {
    provide: DATAPATH,
    useValue: 'api/Users/',
  },],
})

export class UserRegisterComponent extends BaseFormComponent implements OnInit {

  result!: Result;

  // Router and UserResgitrationService injection
  constructor(
    @Inject(IDataService) private userResgistrationService: IDataService<UserInfo, string, UserRegistration>,
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

