import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {UserService} from './user.service';
import {Observable} from 'rxjs';
import ApiParams from '../shared/api-params.json';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private localHost = ApiParams['localHost']

  constructor(private userService: UserService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentUser = this.userService.currentUserValue
    // @ts-ignore
    const isLoggedIn = currentUser && currentUser.token
    const isApiUrl = req.url.startsWith(this.localHost)
    if (isLoggedIn && isApiUrl) {
      req = req.clone({
        setHeaders: {
          // @ts-ignore
          Authorization: `Bearer ${currentUser.token}`
        }
      })
    }
    return next.handle(req)
  }
}
