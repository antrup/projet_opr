import { Component, OnInit } from '@angular/core';
import { TicketService, TICKETSPATH } from './ticket.service';
import { TicketsComponent } from './tickets.component';
import { AuthService } from '../auth/auth.service'
import { HttpClient } from '@angular/common/http';
import { ApplicationService } from '../shared-services/application.service';

@Component({
  selector: 'app-tickets-by-creator',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService, {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/creator/me',
  },],
})
export class TicketsByCreatorComponent extends TicketsComponent {

  constructor(public override ticketService: TicketService,
    public override authService: AuthService) {
    super(ticketService, authService);
}
}
