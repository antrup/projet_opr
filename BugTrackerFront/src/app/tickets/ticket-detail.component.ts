import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { UserService } from '../shared-services/user.service';
import { Ticket } from './interfaces/ticket';
import { TicketService, TICKETSPATH } from './ticket.service';

@Component({
  selector: 'app-ticket-detail',
  templateUrl: './ticket-detail.component.html',
  styleUrls: ['./ticket-detail.component.css'],
  providers: [TicketService, {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/',
  },],
})
export class TicketDetailComponent implements OnInit {

  title?: string;
  form!: FormGroup;
  ticket?: Ticket;
  creatorUserName?: string;

  isDev: boolean = false;

  ownerUserName = "To be attributed";

  // Allow access to elements from template
  @ViewChild("myModal")
  modal!: ElementRef;
  @ViewChild("screenshot")
  img!: ElementRef;
  @ViewChild("img_mod")
  modalImg!: ElementRef;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private httpclient: HttpClient,
    public authService: AuthService,
    public ticketService: TicketService,
    public userService: UserService) { }

  ngOnInit(): void {
    this.isDev = this.authService.isDevUser();
    this.form = new FormGroup({
      id: new FormControl(''),
      subject: new FormControl(''),
      application: new FormControl(''),
      state: new FormControl(''),
      description: new FormControl(''),
      creatorId: new FormControl(''),
      ownerId: new FormControl(''),
      creationDate: new FormControl(''),
      screenshot: new FormControl(''),
    });

    this.loadTicket();
  }

  async loadTicket() {
    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    var id = idParam ? +idParam : 0;

    // get the ticket from the server
    this.ticketService.get(id).subscribe(async result => {
      this.ticket = result;
      this.title = "Details - Ticket " + this.ticket.id;

      // update the form with the ticket value
      this.form.patchValue(this.ticket);

      // get creator username
      this.creatorUserName = await this.getUserName(this.form.controls['creatorId'].value);

      // get owner username, if any
      if (this.form.controls["ownerId"].value != null)
        this.ownerUserName = await this.getUserName(this.form.controls['ownerId'].value);
    });
  }

  async getUserName(id: string): Promise<string> {
    var user = await this.userService.get(id).toPromise();
    if (user)
      return user.userName;
    else
      return "User not found";
  }

   // take over a ticket (register as "owner")
  takeTicket() {
    this.ticketService.takeTicket(this.ticket!.id).subscribe(result => {
      console.log("Ticket " + this.ticket!.id + " has been taken.");
      this.router.navigate(['/']);
    }, error => console.error(error));
  }

  // close a ticket (only for owner)
  closeTicket() {
    this.ticketService.closeTicket(this.ticket!.id).subscribe(result => {
      console.log("Ticket " + this.ticket!.id + " has been closed.");
      this.router.navigate(['/']);
    }, error => console.error(error));
  }

  // Get the image and insert it inside the modal
    openModal() {
    this.modal.nativeElement.style.display = "block";
    this.modalImg.nativeElement.src = this.img.nativeElement.src;
    this.modalImg.nativeElement.alt = this.img.nativeElement.alt;
  }

  closeModal() {
    this.modal.nativeElement.style.display = "none";
  }
}

