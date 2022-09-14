import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { IAuthService } from './Iauth-service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private authService: IAuthService,
    private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // get the auth token
    var token = this.authService.getToken();

    // if the token is present, clone the request and add the auth header
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: "Bearer " + token.toString()
        }
      });
    }

    // send the request to the next handler
    return next.handle(req).pipe(
      catchError((error) => {

        // Logout on 401
        if (error instanceof HttpErrorResponse && error.status === 401) {
          this.authService.logout();
          this.router.navigate(['login']);
        }
        return throwError(error);
      })
    );
  }
}
