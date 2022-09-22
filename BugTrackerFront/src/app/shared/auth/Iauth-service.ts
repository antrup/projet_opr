import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "./auth.service";
import { LoginRequest } from "./interfaces/login-request";
import { LoginResult } from "./interfaces/login-result";

@Injectable({
  providedIn: 'root',
  useClass: AuthService
})
export abstract class IAuthService {

  // Subjects based on user status

  public abstract authStatus: Observable<boolean>
  public abstract isAdmin: Observable<boolean>
  public abstract isDev: Observable<boolean>

  // check user status at init
  public abstract init(): void

  // TRUE if an authentification token is present, FALSE otherwise
  public abstract isAuthenticated(): boolean

  // TRUE if DevUser role is present in roles, FALSE otherwise
  public abstract isDevUser(): boolean

  // TRUE if AdminUser role is present in roles, FALSE otherwise
  public abstract isAdminUser(): boolean

  // Return token , if any
  public abstract getToken(): string | null

  // Return userID , if any
  public abstract getID(): string | null

  // Return roles , if any
  public abstract getRoles(): string | null

  // Send login request to server
  public abstract login(item: LoginRequest): Observable<LoginResult>

  // Clean on logout
  public abstract logout(): void
}
