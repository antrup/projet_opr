import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Stats } from './interfaces/stats';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class StatsService {

  constructor(
    protected http: HttpClient) { }

  get(): Observable<Stats> {
    var url = environment.baseUrl + "api/Stats/";
    return this.http.get<Stats>(url);
  }

}


