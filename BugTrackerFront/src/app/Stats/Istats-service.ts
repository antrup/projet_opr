import { Observable } from "rxjs";
import { Stats } from "./interfaces/stats";


export abstract class IStatsService {

  abstract get(): Observable<Stats>;
}
