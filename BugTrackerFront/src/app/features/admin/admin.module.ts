import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { ApplicationComponent } from './application.component';
import { ApplicationAddComponent } from './application-add.component';
import { UsersComponent } from './users.component';
import { UserRegisterComponent } from './user-register.component';
import { SharedModule } from '../../shared/shared.module';



@NgModule({
  declarations: [
    AdminComponent,
    ApplicationComponent,
    ApplicationAddComponent,
    UsersComponent,
    UserRegisterComponent,
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class AdminModule { }
