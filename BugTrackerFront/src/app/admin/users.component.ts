import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UserInfo } from '../shared-services/interfaces/userinfo';
import { UserService } from '../shared-services/user.service';
import { BaseTableComponent } from '../base-table.component';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
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
  constructor(public userService: UserService) {
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
