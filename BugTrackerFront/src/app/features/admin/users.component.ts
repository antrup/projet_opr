import { AfterViewInit, Component, Inject, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BaseTableComponent } from '../../shared/base-table.component';
import { DATAPATH, DataService } from '../../shared/data.service';
import { IDataService } from '../../shared/Idata-service';
import { UserInfo } from '../../shared/interfaces/userinfo';
import { UserRegistration } from './interfaces/userRegistration';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
  providers: [{
    provide: IDataService,
    useClass: DataService<UserInfo, string, UserRegistration>,
  },
  {
    provide: DATAPATH,
    useValue: 'api/Users/',
  },],
})

export class UsersComponent extends BaseTableComponent<UserInfo> implements AfterViewInit {

  // Table default parameters
  columnsToDisplay = ['ID', 'name', 'email', 'dev_r', 'admin_r', 'delete'];
  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "userName";
  public defaultSortOrder: "asc" | "desc" = "asc";

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // UserService injection
  constructor(@Inject(IDataService) public userService: IDataService<UserInfo, string, UserRegistration>) {
    super();
  }

  ngAfterViewInit(): void {
    this.loadData();
  }

  loadData() {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    this.getData(pageEvent, this.sort, this.paginator, this.defaultSortColumn, this.defaultSortOrder, this.userService);
  }

  // method binded to delete button
  async deleteUser(id: string) {
    this.userService.delete(id).subscribe(() => {
      console.log("User " + id + " has been deleted.");
      this.loadData();
    }, error => console.error(error));
  }

  checkAdmin(user: UserInfo): boolean {
    return (user.roles.some((role => role === "Administrator")));
  }

  checkDev(user: UserInfo): boolean {
    return (user.roles.some((role => role === "DevUser")));
  }
}                       
