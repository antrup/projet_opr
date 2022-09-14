import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../base-form.component';
import { DATAPATH, DataService } from '../shared-services/data.service';
import { IDataService } from '../shared-services/Idata-service';
import { Application } from '../shared-services/interfaces/application';
import { newApplication } from './interfaces/newApplication';


@Component({
  selector: 'app-application-add',
  templateUrl: './application-add.component.html',
  styleUrls: ['./application-add.component.css'],
  providers: [
    {
      provide: IDataService,
      useClass: DataService<Application, number, newApplication>,
    },

    {
      provide: DATAPATH,
      useValue: 'api/Applications/',
    },],
})

export class ApplicationAddComponent extends BaseFormComponent implements OnInit {

  // Router and ApplicationService injection
  constructor(
    private router: Router,
    @Inject(IDataService) private applicationService: IDataService<Application, number, newApplication>) {
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
    let application: newApplication = { name: this.form.controls["name"].value };

    this.applicationService
      .post(application)
      .subscribe(() => {
        console.log("Application " + application.name + " has been created.");
        this.router.navigate(['/admin']);
      }, error => console.error(error));
  }
}
