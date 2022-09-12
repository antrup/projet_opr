import { Component, NgZone } from '@angular/core';
import { FormGroup, AbstractControl } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ApiResult, BaseService } from './base.service';

@Component({
  template: ''
})
export abstract class BaseTableComponent<T> {

  dataSource?: MatTableDataSource<T>;

  constructor() { }

  getData(event: PageEvent,
    sort: MatSort,
    paginator: MatPaginator,
    defaultSortColumn: string,
    defaultSortOrder: string,
    dataservice: BaseService<T>) {
    var sortColumn = (sort)
      ? sort.active
      : defaultSortColumn;
    var sortOrder = (sort)
      ? sort.direction
      : defaultSortOrder;
    dataservice.getData(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder)
      .subscribe((result: ApiResult<T>) => {
        paginator.length = result.totalCount;
        paginator.pageIndex = result.pageIndex;
        paginator.pageSize = result.pageSize;
        this.dataSource = new MatTableDataSource<T>(result.data);
      }, (error: any) => console.error(error));
  }

  
}
