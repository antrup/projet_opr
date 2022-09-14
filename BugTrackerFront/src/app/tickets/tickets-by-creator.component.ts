import { Component, Inject } from '@angular/core';
import { APPLICATIONSERVICE, TicketService, TICKETSPATH } from './ticket.service';
import { TicketsComponent } from './tickets.component';
import { DATAPATH, DataService } from '../shared-services/data.service';
import { Application } from '../shared-services/interfaces/application';
import { newApplication } from '../admin/interfaces/newApplication';
import { ITicketService } from './Iticket-service';
import { IAuthService } from '../auth/Iauth-service';


@Component({
  selector: 'app-tickets-by-creator',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [{
    provide: ITicketService,
    useClass: TicketService
  },
  {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/creator/me',
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
export class TicketsByCreatorComponent extends TicketsComponent {

  constructor(@Inject(ITicketService) public override ticketService: ITicketService,
    public override authService: IAuthService) {
    super(ticketService, authService);
}
}
