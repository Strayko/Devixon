import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';
import {ILoggedUser} from './logged-user';
import ApiParams from '../shared/api-params.json';
import {User} from './user-details';

@Injectable()
export class UserService {
  private localHost = ApiParams['localHost']
  private currentUserSubject: BehaviorSubject<User>
  public currentUser: Observable<User>

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')))
    this.currentUser = this.currentUserSubject.asObservable()
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  save(user) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post<ILoggedUser>(this.localHost + '/api/user/register', user, options)
      .pipe(
        map((data: ILoggedUser) => {
          return data
        }), catchError(this.handleError<ILoggedUser>('saveUser')))
  }

  login(user) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post<ILoggedUser>(this.localHost + '/api/user/login', user, options)
      .pipe(
        map((data) => {
          if (data != null) {
            localStorage.setItem('currentUser', JSON.stringify(data))
            // @ts-ignore
            this.currentUserSubject.next(data)
            return data
          }
          return catchError(this.handleError('loginUser'))
        })
      )
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error)
      return of(result as T)
    }
  }
}
