import { Inject, Injectable, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Ticket } from './interfaces/ticket';
import { newApplication } from '../admin/interfaces/newApplication';
import { ITicketService } from './Iticket-service';
import { IDataService } from '../../shared/Idata-service';
import { Application } from '../../shared/interfaces/application';
import { DataService } from '../../shared/data.service';

export const TICKETSPATH = new InjectionToken<string>('path');
export const APPLICATIONSERVICE = new InjectionToken<IDataService<Application, number, newApplication>>('application service');

@Injectable()
export class TicketService extends DataService<Ticket, number, FormData> implements ITicketService {

  applications?: Application[];

  constructor(
    @Inject(TICKETSPATH) path: string,
    http: HttpClient,
    @Inject(APPLICATIONSERVICE) private applicationService: IDataService<Application, number, newApplication>) {
    super(path, http,)
    this.loadApplications();
  }

  // take over a ticket (register as "owner")
  takeTicket(id: number) {
    return this.http.get(this.getUrl(this._apiPath + "take/" + id));
  }

  // close a ticket (only for owner)
  closeTicket(id: number) {
      return this.http.get(this.getUrl(this._apiPath + "close/" + id));
  }

  async loadApplications() {
    this.applications = await this.applicationService.getDataNoPagination("name", "DESC");
  }

}


