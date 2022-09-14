import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { NgModule } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AuthInterceptor } from './auth/auth.interceptor';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { AppRoutingModule } from './app-routing/app-routing.module';

import { ReactiveFormsModule } from '@angular/forms';
import { TicketDetailComponent } from './tickets/ticket-detail.component';
import { TicketCreateComponent } from './tickets/ticket-create.component';
import { TicketsComponent } from './tickets/tickets.component';
import { MaterialModule } from './material.module';
import { LoginComponent } from './auth/login.component';
import { AdminComponent } from './admin/admin.component';
import { ApplicationComponent } from './admin/application.component';
import { ApplicationAddComponent } from './admin/application-add.component';
import { UsersComponent } from './admin/users.component';
import { UserRegisterComponent } from './admin/user-register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { LayoutModule } from '@angular/cdk/layout';
import { TicketStatsComponent } from './Stats/ticket-stats.component';
import { MyticketsStatsComponent } from './Stats/mytickets-stats.component';
import { TicketsByCreatorComponent } from './tickets/tickets-by-creator.component';
import { TicketsByOwnerComponent } from './tickets/tickets-by-owner.component';
import { TicketsOwnedStatsComponent } from './Stats/tickets-owned-stats.component';
import { IAuthService } from './auth/Iauth-service';
import { AuthService } from './auth/auth.service';
import { IStatsService } from './Stats/Istats-service';
import { StatsService } from './Stats/stats.service';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavMenuComponent,
    TicketDetailComponent,
    TicketCreateComponent,
    TicketsComponent,
    LoginComponent,
    AdminComponent,
    ApplicationComponent,
    ApplicationAddComponent,
    UsersComponent,
    UserRegisterComponent,
    DashboardComponent,
    TicketStatsComponent,
    MyticketsStatsComponent,
    TicketsByCreatorComponent,
    TicketsByOwnerComponent,
    TicketsOwnedStatsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    LayoutModule,
  ],
  providers: [
    {
      provide: IAuthService,
      useClass: AuthService
    },
    {
      provide: IStatsService,
      useClass: StatsService
    },
    {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  },
   ],
  bootstrap: [AppComponent]
})
export class AppModule { }
