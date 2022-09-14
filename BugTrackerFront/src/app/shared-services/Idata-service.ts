import { Observable } from "rxjs";
import { ApiResult } from "./data.service";

export abstract class IDataService<T, IDTYPE, POSTTYPE> {

  abstract getData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string
  ): Observable<ApiResult<T>>

  // return data without pagination from an API offering pagination
  abstract getDataNoPagination(sortcolumn: string, sortorder: string): Promise<T[] | undefined>

  abstract get(id: IDTYPE): Observable<T>

  abstract put(item: T, id: IDTYPE): Observable<T>

  abstract post(item: POSTTYPE): Observable<any>

  abstract delete(id: IDTYPE): Observable<any>


}
