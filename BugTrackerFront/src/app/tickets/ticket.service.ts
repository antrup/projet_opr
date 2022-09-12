import { Inject, Injectable, InjectionToken, NgModule } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResult, BaseService } from '../base.service';
import { Ticket } from './interfaces/ticket';
import { Application } from '../shared-services/interfaces/application';
import { ApplicationService } from '../shared-services/application.service';

export const TICKETSPATH = new InjectionToken<string>('path');

@Injectable()
export class TicketService extends BaseService<Ticket> {

  applications?: Application[];

  constructor(
    @Inject(TICKETSPATH) path: string,
    http: HttpClient,
    private applicationService: ApplicationService) {
    super(path, http,)
    this.loadApplications();
  }

  // override methods to apply stricter typing
  override get(id: number): Observable<Ticket> {
    return super.get(id);
  }

  override delete(id: number): Observable<Object> {
    return super.delete(id);
  }

  override put(item: Ticket, id: number): Observable<Ticket> {
    return super.put(item, id);
  }

  override post(item: FormData): Observable<Ticket> {
    return super.post(item);
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


