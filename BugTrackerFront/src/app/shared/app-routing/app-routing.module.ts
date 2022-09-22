import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from '../../features/admin/admin.component';
import { ApplicationAddComponent } from '../../features/admin/application-add.component';
import { UserRegisterComponent } from '../../features/admin/user-register.component';
import { TicketCreateComponent } from '../../features/tickets/ticket-create.component';
import { TicketDetailComponent } from '../../features/tickets/ticket-detail.component';
import { AdminGuard } from '../auth/admin.guard';
import { AuthGuard } from '../auth/auth.guard';
import { LoginComponent } from '../auth/login.component';
import { HomeComponent } from '../../core/home/home.component';

// App routes definition
const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'new_ticket', component: TicketCreateComponent, canActivate: [AuthGuard] },
  { path: 'ticket/:id', component: TicketDetailComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'add_application', component: ApplicationAddComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'register_user', component: UserRegisterComponent, canActivate: [AuthGuard, AdminGuard] },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [RouterModule]
})


export class AppRoutingModule { }
