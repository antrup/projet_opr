import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminModule } from './admin/admin.module';
import { StatsModule } from './stats/stats.module';
import { TicketsModule } from './tickets/tickets.module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AdminModule,
    StatsModule,
    TicketsModule
  ],
  exports: [
    AdminModule,
    StatsModule,
    TicketsModule
  ]
})
export class FeaturesModule { }
