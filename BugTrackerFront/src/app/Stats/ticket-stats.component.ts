import { Component, OnInit } from '@angular/core';
import { Stats } from './interfaces/stats';
import { IStatsService } from './Istats-service';

@Component({
  selector: 'app-ticket-stats',
  templateUrl: './ticket-stats.component.html',
  styleUrls: ['./ticket-stats.component.css']
})
// Stats regarding all tickets
export class TicketStatsComponent implements OnInit {

  stats?: Stats;

  constructor(public statsService: IStatsService) { }

  ngOnInit(): void {
    this.statsService.get().subscribe(result => this.stats = result);
  }

}
