import { Component, Inject } from '@angular/core';
import { newApplication } from '../admin/interfaces/newApplication';
import { AuthService } from '../auth/auth.service';
import { IAuthService } from '../auth/Iauth-service';
import { DATAPATH, DataService } from '../shared-services/data.service';
import { Application } from '../shared-services/interfaces/application';
import { ITicketService } from './Iticket-service';
import { APPLICATIONSERVICE, TicketService, TICKETSPATH } from './ticket.service';
import { TicketsComponent } from './tickets.component';

@Component({
  selector: 'app-tickets-by-owner',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [{
    provide: ITicketService,
    useClass: TicketService
  },
  {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/owner/me',
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

export class TicketsByOwnerComponent extends TicketsComponent {

  constructor(@Inject(ITicketService) public override ticketService: ITicketService,
    public override authService: IAuthService) {
    super(ticketService, authService);

  }
}
