import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketDetailComponent } from './ticket-detail.component';
import { TicketCreateComponent } from './ticket-create.component';
import { TicketsComponent } from './tickets.component';
import { SharedModule } from '../../shared/shared.module';
import { TicketsByCreatorComponent } from './tickets-by-creator.component';
import { TicketsByOwnerComponent } from './tickets-by-owner.component';



@NgModule({
  declarations: [
    TicketDetailComponent,
    TicketCreateComponent,
    TicketsComponent,
    TicketsByCreatorComponent,
    TicketsByOwnerComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [
    TicketDetailComponent,
    TicketCreateComponent,
    TicketsComponent,
    TicketsByCreatorComponent,
    TicketsByOwnerComponent,
  ]
})
export class TicketsModule { }
