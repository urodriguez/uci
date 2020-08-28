import {EventEmitter, Injectable} from '@angular/core';
import {User} from './pages/users/shared/user.model';
import {SecurityToken} from './auth/shared/security-token.model';

@Injectable({
  providedIn: 'root'
})
export class AppContext {
  private _loggedUser: User;
  set loggedUser(loggedUser: User) {
    this._loggedUser = loggedUser;
    this.loggedUserChanged.emit(this._loggedUser);
  }
  get loggedUser(){
    return this._loggedUser;
  }
  loggedUserChanged = new EventEmitter<User>();

  private _securityToken: SecurityToken;
  set securityToken(securityToken: SecurityToken) {
    this._securityToken = securityToken;
    this.securityTokenChanged.emit(this._securityToken);
  }
  get securityToken(){
    return this._securityToken;
  }
  securityTokenChanged = new EventEmitter<SecurityToken>();
}
