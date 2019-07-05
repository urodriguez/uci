import { Injectable, EventEmitter } from '@angular/core';
import { Login } from './login.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  logginSuccess = new EventEmitter<boolean>();
  checkingUserCredentials = new EventEmitter<boolean>();

  constructor() { }

  checkUserCredentials(login: Login) {
    this.checkingUserCredentials.emit(true);
    setTimeout(() => {
      this.logginSuccess.emit(login.userName === 'admin' && login.password === 'admin');
    }, 1500);
  }
}
