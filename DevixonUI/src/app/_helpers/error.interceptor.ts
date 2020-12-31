import {Injectable} from '@angular/core';
import {HttpInterceptor, HttpRequest, HttpHandler, HttpEvent} from '@angular/common/http';
import {UserService} from '../shared/user.service';
import {Observable, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import ApiParams from '../shared/api-params.json';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  urlsToNotUse: Array<string>
  private localHost = ApiParams['localHost']

  constructor(private userService: UserService) {
    this.urlsToNotUse = [
      this.localHost + '/api/user/login'
    ]
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let positionUrl = this.urlsToNotUse

    if (positionUrl.indexOf(req.url) >= 0) {
      return next.handle(req).pipe(catchError(err => {
        const error = err.status
        return throwError(error)
      }))
    }

    return next.handle(req).pipe(catchError(err => {
      if (err.status === 401) {
        this.userService.logout()
        location.reload(true)
      }

      const error = err.status
      return throwError(error)
    }))
  }
}
