import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from '../admin/admin.component';
import { AdminGuard } from '../admin/admin.guard';
import { ApplicationAddComponent } from '../admin/application-add.component';
import { UserRegisterComponent } from '../admin/user-register.component';
import { AuthGuard } from '../auth/auth.guard';
import { LoginComponent } from '../auth/login.component';
import { HomeComponent } from '../home/home.component';
import { TicketCreateComponent } from '../tickets/ticket-create.component';
import { TicketDetailComponent } from '../tickets/ticket-detail.component';

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
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
