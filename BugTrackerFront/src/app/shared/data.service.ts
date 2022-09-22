import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IDataService } from './Idata-service';

export const DATAPATH = new InjectionToken<string>('path');

@Injectable()
export class DataService<T, IDTYPE, POSTTYPE> implements IDataService<T, IDTYPE, POSTTYPE> {

  protected _apiPath: string;

  constructor(@Inject(DATAPATH) apiPath:string,
    protected http: HttpClient) {
    this._apiPath = apiPath;
  }

  getData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string
  ): Observable<ApiResult<T>> {
    var url = this.getUrl(this._apiPath);
    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);
    return this.http.get<ApiResult<T>>(url, { params });
  }

  // return data without pagination from an API offering pagination
  async getDataNoPagination(sortcolumn: string, sortorder: string = "DESC") {
    // Ask for 9999 entries
    var result = await this.getData(0, 9999, sortcolumn, sortorder).toPromise();
    // If total entries > 9999, ask for all the entries
    if (result && result?.totalCount > 9999)
      result = await this.getData(0, result?.totalCount, "name", "DESC").toPromise();
    // return only the data
    return (result?.data);
  }

  get(id: IDTYPE): Observable<T> {
    var url = this.getUrl(this._apiPath + id);
    return this.http.get<T>(url);
  }

  put(item: T, id: IDTYPE): Observable<T> {
    var url = this.getUrl(this._apiPath + id);
    return this.http.put<T>(url, item);
  }

  post(item: POSTTYPE): Observable<any> {
    var url = this.getUrl(this._apiPath);
    return this.http.post(url, item);
  }

  delete(id: IDTYPE): Observable<any> {
    var url = this.getUrl(this._apiPath + id);
    return this.http.delete(url);
  }

  protected getUrl(url: string) {
    return environment.baseUrl + url;
  }
}

export interface ApiResult<T> {
  data: T[];
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  sortColumn: string;
  sortOrder: string;
}
