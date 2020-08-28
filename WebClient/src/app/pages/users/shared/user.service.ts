import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import {catchError, map} from 'rxjs/operators';

import {User} from './user.model';
import {ResetPassword} from '../../../auth/reset-password/reset-password.model';
import {AppContext} from '../../../app-context';
import {UserFactory} from './user.factory';
import {BaseHttpService} from '../../../shared/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseHttpService {

  private readonly usersApiURL = `${this.baseApiURL}/users`;

  constructor(httpClient: HttpClient,
              appContext: AppContext,
              private readonly userFactory: UserFactory) {
    super(httpClient, appContext);
  }

  getUser(id: string): Observable<User> {
    return this.httpClient.get<User>(`${this.usersApiURL}/${id}`, this.getHttpOptions()).pipe(
      map(response => this.userFactory.create(response)),
      catchError(this.handleError)
    );
  }

  getAll(): Observable<User[]> {
    return this.httpClient.get<User[]>(this.usersApiURL, this.getHttpOptions()).pipe(
      map(response => this.userFactory.createList(response)),
      catchError(this.handleError)
    );
  }

  create(user: User): Observable<string> {
    return this.httpClient.post<string>(this.usersApiURL, user, this.getHttpOptions()).pipe(
      catchError(this.handleError)
    );
  }

  update(user: User) {
    return this.httpClient.put(`${this.usersApiURL}/${user.id}`, user, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  delete(user: User) {
    return this.httpClient.delete(`${this.usersApiURL}/${user.id}`, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  resetPassword(resetPassword: ResetPassword) {
    return this.httpClient.patch(
      `${this.usersApiURL}/logged/resetPassword`,
      resetPassword,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  getLogged(): Observable<User> {
    return this.httpClient.get<User>(`${this.usersApiURL}/logged`, this.getHttpOptions()).pipe(
      map(response => this.userFactory.create(response)),
      catchError(this.handleError)
    );
  }

  forgotPassword(email: string) {
    return this.httpClient.patch(
      `${this.usersApiURL}/${email}/forgotPassword`,
      {}
    ).pipe(
      catchError(this.handleError)
    );
  }
}
