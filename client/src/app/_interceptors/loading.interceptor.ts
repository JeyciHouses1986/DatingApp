import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, delay, finalize, identity } from "rxjs";
import { BusyService } from "../_services/busy.service";
import { environment } from "../../environments/environment";

@Injectable()
export class LoadingInterceptor implements HttpInterceptor{

  constructor(private busyService : BusyService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.busyService.busy();
    return next.handle(req).pipe(
      (environment.production ? identity : delay(3000)),
      finalize(() => {
        this.busyService.idle();
      })
    );
  }

}