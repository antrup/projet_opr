import { Component, OnInit } from '@angular/core';
import { Stats } from './interfaces/stats';
import { StatsService } from './stats.service';

@Component({
  selector: 'app-mytickets-stats',
  templateUrl: './mytickets-stats.component.html',
  styleUrls: ['./ticket-stats.component.css']
})
// Stats regarding tickets created by the user
export class MyticketsStatsComponent implements OnInit {

  stats?: Stats;

  constructor(public statsService: StatsService) { }

  ngOnInit(): void {
    this.statsService.get().subscribe(result => this.stats = result);
  }

}

