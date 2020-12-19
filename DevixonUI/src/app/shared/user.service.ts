import {EventEmitter, Injectable} from '@angular/core';
import {Observable, of, pipe, Subject} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {catchError} from 'rxjs/operators';

@Injectable()
export class UserService {
  constructor(private http: HttpClient) {
  }

  saveEvent(event) {
    let options = {headers: new HttpHeaders({'Content-Type': 'application/json'})}
    return this.http.post('/api/events', event, options)
      .pipe(catchError(this.handleError('saveEvent')))
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error)
      return of(result as T)
    }
  }
}
