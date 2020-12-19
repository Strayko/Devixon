import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';
import {LoggedUser} from './logged-user';

@Injectable()
export class UserService {
  constructor(private http: HttpClient) {
  }

  saveUser(user) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post('https://localhost:5001/api/user/register', user, options)
      .pipe(
        map((data: LoggedUser) => {
          console.log(data)
          return data
        }), catchError(this.handleError('saveUser')))
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error)
      return of(result as T)
    }
  }
}
