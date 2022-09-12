import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfo } from './interfaces/userinfo';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseService<UserInfo> {

  constructor(
    http: HttpClient) {
    super("api/Users/", http);
  }

  // override methods to apply stricter typing
  override get(id: string): Observable<UserInfo> {
    return super.get(id);
  }

  override delete(id: string): Observable<Object> {
    return super.delete(id);
  }

  override put(item: UserInfo, id: string): Observable<UserInfo> {
    return super.put(item, id);
  }
}

