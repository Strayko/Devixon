import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';
import {ILoggedUser} from './logged-user';
import ApiParams from '../shared/api-params.json';

@Injectable()
export class UserService {
  private localHost = ApiParams['localHost']
  constructor(private http: HttpClient) {
  }

  save(user) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post<ILoggedUser>(this.localHost + '/api/user/register', user, options)
      .pipe(
        map((data: ILoggedUser) => {
          console.log(data)
          return data
        }), catchError(this.handleError<ILoggedUser>('saveUser')))
  }

  login(user) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post<ILoggedUser>(this.localHost + '/api/user/login', user, options)
      .pipe(
        map((data: ILoggedUser) => {
          if (data != null) {
            return data
          }
          return catchError(this.handleError('loginUser'))
        })
      )
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error)
      return of(result as T)
    }
  }
}
