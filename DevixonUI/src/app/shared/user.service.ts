import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';
import {ILoggedUser} from '../_interface/logged-user';
import ApiParams from '../shared/api-params.json';
import {IUser} from '../_interface/user-details';
import {IToken} from '../_interface/token';
import {Router} from '@angular/router';

@Injectable()
export class UserService {
  private localHost = ApiParams['localHost']
  private currentUserSubject: BehaviorSubject<ILoggedUser>
  public currentUser: Observable<ILoggedUser>

  constructor(private http: HttpClient, private router: Router) {
    this.currentUserSubject = new BehaviorSubject<ILoggedUser>(JSON.parse(localStorage.getItem('currentUser')))
    this.currentUser = this.currentUserSubject.asObservable()
  }

  public get currentUserValue(): ILoggedUser {
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

  validate(token) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post<IToken>(this.localHost + '/api/user/validate', token, options)
      .pipe(
        map((data) => {
          return data
        })
      )
  }

  details() {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.get<IUser>(this.localHost + '/api/user/details', options)
      .pipe(
        map((data) => {
            if (data != null) {
              return data
            }
            return catchError(this.handleError('user-details'))
          }
        )
      )
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/'])
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error)
      return of(result as T)
    }
  }
}
