import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IAuthService } from './auth/Iauth-service';
import { AuthService } from './auth/auth.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth/auth.interceptor';
import { LoginComponent } from './auth/login.component';
import { MaterialModule } from './material.module';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';
import { AuthGuard } from './auth/auth.guard';
import { AdminGuard } from './auth/admin.guard';
import { Router, RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing/app-routing.module';


@NgModule({
  declarations: [
    LoginComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    LayoutModule,
    AppRoutingModule,
    RouterModule,
  ],
  exports: [
    LoginComponent,
    MaterialModule,
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    LayoutModule,
    AppRoutingModule,
    RouterModule,
  ],
  providers: [
    {
      provide: IAuthService,
      useClass: AuthService
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    ]
})
export class SharedModule { }
