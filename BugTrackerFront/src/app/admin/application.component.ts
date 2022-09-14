import { AfterViewInit, Component, Inject, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Application } from '../shared-services/interfaces/application';
import { BaseTableComponent } from '../base-table.component';
import { DATAPATH, DataService } from '../shared-services/data.service';
import { newApplication } from './interfaces/newApplication';
import { IDataService } from '../shared-services/Idata-service';
import { UserInfo } from '../shared-services/interfaces/userinfo';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css'],
  providers: [
    {
    provide: IDataService,
    useClass: DataService<Application, number, newApplication>,
  },

    {
    provide: DATAPATH,
    useValue: 'api/Applications/',
  },],
})

export class ApplicationComponent extends BaseTableComponent<Application> implements AfterViewInit {

  // Table default parameters
  columnsToDisplay = ['ID', 'name', 'delete'];
  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "name";
  public defaultSortOrder: "asc" | "desc" = "asc";

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // ApplicationService injection
  constructor(@Inject(IDataService) public applicationService: IDataService<Application, number, newApplication>) {
    super();
  }

  ngAfterViewInit(): void {
    this.loadData();
  }

  loadData() {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    // getData method inherited from BaseTableComponent
    this.getData(pageEvent, this.sort, this.paginator, this.defaultSortColumn, this.defaultSortOrder, this.applicationService);
  }

  async deleteApplication(id: number) {
    this.applicationService.delete(id).subscribe(() => {
      console.log("Application " + id + " has been deleted.");
      this.loadData();
    }, error => console.error(error));
  }
}                       



