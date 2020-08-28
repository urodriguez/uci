import { Injectable } from '@angular/core';
import {UserCredential} from '../login/user-credential.model';
import {Observable} from 'rxjs';
import {LoginResult} from '../login/login-result.model';
import {HttpClient} from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';

import {BaseHttpService} from '../../shared/base-http.service';
import {AppContext} from '../../app-context';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseHttpService {
  private readonly tokensApiURL = `${this.baseApiURL}/tokens`;

  constructor(httpClient: HttpClient,
              appContext: AppContext) {
    super(httpClient, appContext);
  }

  login(userCredential: UserCredential, rememberUser: boolean = false): Observable<LoginResult> {
    return this.httpClient.post<LoginResult>(this.tokensApiURL, userCredential)
      .pipe(
        map(response => {
          const loginResult = new LoginResult(response.status, response.securityToken);
          localStorage.removeItem('securityToken');//if another user was logged on same browser
          this.appContext.securityToken = loginResult.securityToken;
          if (rememberUser) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('securityToken', JSON.stringify(loginResult.securityToken));
          }
          return loginResult;
        }),
        catchError(this.handleError)
      );
  }

  logout() {
    localStorage.removeItem('securityToken');
  }
}
