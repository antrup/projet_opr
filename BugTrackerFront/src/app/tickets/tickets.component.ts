import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Ticket } from './interfaces/ticket';
import { TicketService, TICKETSPATH } from './ticket.service';
import { BaseTableComponent } from '../base-table.component';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService, {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/',
  },],
})
export class TicketsComponent extends BaseTableComponent<Ticket> implements OnInit {
  applicationsMap!: Map<number, string>;

  // Default parameters
  columnsToDisplay = ['application', 'subject', 'creationDate', 'state', 'detail', 'delete'];
  defaultSortColumn: string = "creationDate";
  defaultSortOrder: "asc" | "desc" = "desc";
  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(public ticketService: TicketService,
              public authService: AuthService  ) {
    super();
}
 
  ngOnInit(): void {
    this.loadData();
    // Delete button to be displayed only for dev user
    if (!this.authService.isDevUser())
      this.columnsToDisplay = ['application', 'subject', 'creationDate', 'state', 'detail'];
  }

  async loadData() {
    await this.loadApplications();
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    // getdata: method inherited from BaseTableComponent
    this.getData(pageEvent, this.sort, this.paginator, this.defaultSortColumn, this.defaultSortOrder, this.ticketService);
  }

  // Put applications in a map
  async loadApplications() {
    await this.ticketService.loadApplications();
    let applications = this.ticketService.applications;
    if (applications) {
      this.applicationsMap = new Map();
      applications.forEach(application => this.applicationsMap.set(application.id!, application.name));
    }
  }

  // Method binded to the delete button
  async deleteTicket(id: number) {
    this.ticketService.delete(id).subscribe(result => {
      console.log("Ticket " + id + " has been deleted.");
      this.loadData();
    }, error => console.error(error));
  }
}                       
