import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Ticket } from './interfaces/ticket';
import { APPLICATIONSERVICE, TicketService, TICKETSPATH } from './ticket.service';
import { ITicketService } from './Iticket-service';
import { DATAPATH, DataService } from '../../shared/data.service';
import { Application } from '../../shared/interfaces/application';
import { newApplication } from '../admin/interfaces/newApplication';
import { BaseTableComponent } from '../../shared/base-table.component';
import { IAuthService } from '../../shared/auth/Iauth-service';


@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [{
    provide: ITicketService,
    useClass: TicketService
  },
  {
    provide: TICKETSPATH,
    useValue: 'api/Tickets/',
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

  constructor(@Inject(ITicketService) public ticketService: ITicketService,
              public authService: IAuthService) {
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
