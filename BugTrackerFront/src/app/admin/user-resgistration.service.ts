import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserRegistration } from '../admin/interfaces/userRegistration';
import { Result } from './interfaces/result';
import { environment } from '../../environments/environment';

@Injectable()
export class UserResgitrationService {

  constructor(protected http: HttpClient) {}

  post(item: UserRegistration): Observable<Result> {
    var url = this.getUrl();
    return this.http.post<Result>(url, item);
  }

  protected getUrl() {
    return environment.baseUrl + "api/Users/";
  }
}
