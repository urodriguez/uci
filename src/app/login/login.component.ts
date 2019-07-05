import { Component, OnInit, Input} from '@angular/core';
import { Login } from './shared/login.model';
import { LoginService } from './shared/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  login: Login;

  constructor(private readonly loginService: LoginService) {
    this.login = new Login();
  }

  processLogin() {
    this.loginService.checkUserCredentials(this.login);
  }
}
