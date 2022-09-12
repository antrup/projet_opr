import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { TicketService, TICKETSPATH } from './ticket.service';
import { TicketsComponent } from './tickets.component';

@Component({
  selector: 'app-tickets-by-owner',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService, {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/owner/me',
  },],
})

export class TicketsByOwnerComponent extends TicketsComponent {

  constructor(public override ticketService: TicketService,
    public override authService: AuthService) {
    super(ticketService, authService);

  }
}
