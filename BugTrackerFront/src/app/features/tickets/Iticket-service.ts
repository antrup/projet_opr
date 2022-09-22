import { Observable } from "rxjs";
import { IDataService } from "../../shared/Idata-service";
import { Application } from "../../shared/interfaces/application";
import { Ticket } from "./interfaces/ticket";


export abstract class ITicketService extends IDataService<Ticket, number, FormData>  {

  applications?: Application[];

  // take over a ticket (register as "owner")
  abstract takeTicket(id: number): Observable<Object>

  // close a ticket (only for owner)
  abstract closeTicket(id: number): Observable<Object>

  abstract loadApplications(): Promise<void>

}
