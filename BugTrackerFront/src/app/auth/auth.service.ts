import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, tap } from 'rxjs';
import { environment } from './../../environments/environment';
import { LoginRequest } from './interfaces/login-request';
import { LoginResult } from './interfaces/login-result';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(protected http: HttpClient) { }

  // Define localstorage field name
  private userToken: string = "token";
  private userName: string = "username";
  private userId: string = "userID";
  private roles: string = "roles"

  // Subjects based on user status
  private _authStatus = new Subject<boolean>();
  public authStatus = this._authStatus.asObservable();
  private _isAdmin = new Subject<boolean>();
  public isAdmin = this._isAdmin.asObservable();
  private _isDev = new Subject<boolean>();
  public isDev = this._isDev.asObservable();

  // check user status at init
  init(): void {
    if (this.isAuthenticated())
      this.setAuthStatus(true);
    if (this.isDevUser())
      this.setDevStatus(true);
    if (this.isAdminUser())
      this.setAdminStatus(true);
  }

  // TRUE if an authentification token is present in localstorage, FALSE otherwise
  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  // TRUE if DevUser role is present in roles field in localstorage, FALSE otherwise
  isDevUser(): boolean {
    return JSON.parse(this.getRoles()!)?.some((element: string) => element == "DevUser");
  }

  // TRUE if AdminUser role is present in roles field in localstorage, FALSE otherwise
  isAdminUser(): boolean {
    return JSON.parse(this.getRoles()!)?.some((element: string) => element == "Administrator");
  }

  // Return token from localstorage, if any
  getToken(): string | null {
    return localStorage.getItem(this.userToken);
  }

  // Return userID from localstorage, if any
  getID(): string | null {
    return localStorage.getItem(this.userId);
  }

  // Return roles from localstorage, if any
  getRoles(): string | null {
    return localStorage.getItem(this.roles);
  }

  // Send login request to server and, if successfull, store response data into localstorage (token, userName, userID, roles)
  login(item: LoginRequest): Observable<LoginResult> {
    var url = environment.baseUrl + "api/auth";
    return this.http.post<LoginResult>(url, item)
      .pipe(tap(LoginResult => {
        if (LoginResult.success && LoginResult.token) {
          localStorage.setItem(this.userToken, LoginResult.token);
          localStorage.setItem(this.userName, LoginResult.userName!);
          localStorage.setItem(this.userId, LoginResult.id!);
          localStorage.setItem(this.roles, JSON.stringify(LoginResult.roles!));
          this.setAuthStatus(true);
          this.setAdminStatus(this.isAdminUser());
          this.setDevStatus(this.isDevUser());
        }
      }));
  }

  // Clean localstorage on logout
  logout() {
    localStorage.removeItem(this.userToken);
    localStorage.removeItem(this.userName);
    localStorage.removeItem(this.userId);
    localStorage.removeItem(this.roles);

    this.setAdminStatus(false);
    this.setDevStatus(false);
    this.setAuthStatus(false);
  }

  // AuthStatus setter
  private setAuthStatus(isAuthenticated: boolean): void {
    this._authStatus.next(isAuthenticated);
  }

  // AdminStatus setter
  private setAdminStatus(isAdmin: boolean): void {
    this._isAdmin.next(isAdmin);
  }

  // DevStatus setter
  private setDevStatus(isDev: boolean): void {
    this._isDev.next(isDev);
  }
}
