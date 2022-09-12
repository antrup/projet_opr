import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../base-form.component';
import { ApplicationService } from '../shared-services/application.service';
import { Application } from '../shared-services/interfaces/application';


@Component({
  selector: 'app-application-add',
  templateUrl: './application-add.component.html',
  styleUrls: ['./application-add.component.css']
})
export class ApplicationAddComponent extends BaseFormComponent implements OnInit {

  // Router and ApplicationService injection
  constructor(
    private router: Router,
    private applicationService: ApplicationService) {
    super();
}

  ngOnInit(): void {
    // Initialise form field and associated validators
    this.form = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(15),
        Validators.pattern("^[a-zA-Z0-9àâäéèêëîïôöùûüÿç \-_'\"]+$")]),
    });
  }

  onSubmit() {
    let application: Application = { name: this.form.controls["name"].value };

    this.applicationService
      .post(application)
      .subscribe(() => {
        console.log("Application " + application.name + " has been created.");
        this.router.navigate(['/admin']);
      }, error => console.error(error));
  }
}
