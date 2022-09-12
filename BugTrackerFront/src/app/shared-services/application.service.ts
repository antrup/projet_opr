import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseService } from '../base.service';
import { Application } from './interfaces/application';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService extends BaseService<Application> {

  constructor(
    http: HttpClient) {
    super("api/Applications/", http);
  }

   // override methods to apply stricter typing
  override get(id: number): Observable<Application> {
    return super.get(id);
  }

  override delete(id: number): Observable<Object> {
    return super.delete(id);
  }

  override put(item: Application, id: number): Observable<Application> {
    return super.put(item, id);
  }
}
