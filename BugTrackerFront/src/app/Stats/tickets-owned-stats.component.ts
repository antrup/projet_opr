import { Component, OnInit } from '@angular/core';
import { Stats } from './interfaces/stats';
import { StatsService } from './stats.service';

@Component({
  selector: 'app-tickets-owned-stats',
  templateUrl: './tickets-owned-stats.component.html',
  styleUrls: ['./ticket-stats.component.css']
})
// Stats regarding tickets owned by the user
export class TicketsOwnedStatsComponent implements OnInit {

  stats?: Stats;

  constructor(public statsService: StatsService) { }

  ngOnInit(): void {
    this.statsService.get().subscribe(result => this.stats = result);
  }

}
