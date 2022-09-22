import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SharedModule } from '../shared/shared.module';
import { FeaturesModule } from '../features/features.module';



@NgModule({
  declarations: [
    HomeComponent,
    NavMenuComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FeaturesModule
  ],
  exports: [
    HomeComponent,
    NavMenuComponent,
    DashboardComponent,
  ]
})
export class CoreModule { }
