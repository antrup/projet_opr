import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketStatsComponent } from './ticket-stats.component';
import { TicketsOwnedStatsComponent } from './tickets-owned-stats.component';
import { MyticketsStatsComponent } from './mytickets-stats.component';
import { SharedModule } from '../../shared/shared.module';
import { IStatsService } from './Istats-service';
import { StatsService } from './stats.service';



@NgModule({
  declarations: [
    TicketStatsComponent,
    MyticketsStatsComponent,
    TicketsOwnedStatsComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [
    TicketStatsComponent,
    MyticketsStatsComponent,
    TicketsOwnedStatsComponent
  ],
  providers: [
    {
      provide: IStatsService,
      useClass: StatsService
    }
  ]
})
export class StatsModule { }
