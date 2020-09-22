import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import {catchError, map} from 'rxjs/operators';

import {User} from './user.model';
import {ResetPassword} from '../../../auth/reset-password/reset-password.model';
import {AppContext} from '../../../app-context';
import {UserFactory} from './user.factory';
import {CrudService} from '../../../shared/services/crud.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends CrudService<User> {

  constructor(httpClient: HttpClient,
              appContext: AppContext,
              factory: UserFactory) {
    super(httpClient, appContext, factory);
    this.setResource('Users');
  }

  resetPassword(resetPassword: ResetPassword) {
    return this.httpClient.patch(
      `${this.resourceApiUrl}/logged/resetPassword`,
      resetPassword,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  getLogged(): Observable<User> {
    return this.httpClient.get<User>(`${this.resourceApiUrl}/logged`, this.getHttpOptions()).pipe(
      map(response => this.factory.create(response) as User),
      catchError(this.handleError)
    );
  }

  forgotPassword(email: string) {
    return this.httpClient.patch(
      `${this.resourceApiUrl}/${email}/forgotPassword`,
      {}
    ).pipe(
      catchError(this.handleError)
    );
  }
}
