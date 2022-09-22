import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../../shared/base-form.component';
import { DATAPATH, DataService } from '../../shared/data.service';
import { Application } from '../../shared/interfaces/application';
import { newApplication } from '../admin/interfaces/newApplication';
import { ITicketService } from './Iticket-service';
import { APPLICATIONSERVICE, TicketService, TICKETSPATH } from './ticket.service';

@Component({
  selector: 'app-ticket-create',
  templateUrl: './ticket-create.component.html',
  styleUrls: ['./ticket-create.component.css'],
  providers: [{
    provide: ITicketService,
    useClass: TicketService
  },
  {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/',
  },
  {
    provide: APPLICATIONSERVICE,
    useClass: DataService<Application, number, newApplication>,
  },
  {
    provide: DATAPATH,
    useValue: 'api/Applications/',
  }
  ],
})
export class TicketCreateComponent extends BaseFormComponent implements OnInit {

  ticketData = new FormData();
  @ViewChild('uploadField') uploadField!: ElementRef;
 

  constructor(
    private router: Router,
    @Inject(ITicketService) public ticketService: ITicketService) {
    super();
}

  ngOnInit(): void {
    this.form = new FormGroup({
      subject: new FormControl('', [Validators.required, Validators.maxLength(100), Validators.pattern("^[a-zA-Z0-9àâäéèêëîïôöùûüÿç \\-_'\"]+$")]),
      application: new FormControl('', Validators.required),
      description: new FormControl('', [Validators.required, Validators.maxLength(3000)]),
      screenshot: new FormControl('')
    });
  }

  onFileChange(e: Event) {
    const target = <HTMLInputElement>(e.target);

    if (target.files) {
      if (target.files[0].size / 1024 / 1024 > 2) {
        alert("File size should be less than 2MB");
        this.uploadField.nativeElement.click();
        return;
      }

      var re = /(?:\.([^.]+))?$/;
      /**
      (?:       #   begin non - capturing group
      \.        #   no dot = no match
      (         #   begin capturing group
      [^.]+     #   capture all characters while no dot is found 
      )         #   end capturing group
      )?        #   end non - capturing group, make it optional
      $         #   anchor to the end of the string
      */
      var ext = re.exec(target.files[0].name);
      
      if (ext !== null) {
        var extension = ext[1];
        if (extension != 'jpg') {
          alert("File must be jpg");
          this.uploadField.nativeElement.click();
          return;
        }
       }
      //this.form.controls['screenshot'].patchValue(target.files[0]);
      this.ticketData.append("screenshot", target.files[0]);
    }
  }

  onSubmit() { 
    this.ticketData.append("subject", this.form.controls['subject'].value);
    this.ticketData.append("application", this.form.controls['application'].value);
    this.ticketData.append("description", this.form.controls['description'].value);
    this.ticketService.post(this.ticketData)
      .subscribe(() => {
          console.log("Ticket has been created.");
          this.router.navigate(['/']);
      }, error => console.error(error));
  }
}
