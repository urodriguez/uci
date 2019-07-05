import { Component } from '@angular/core';
import { LoginService } from './login/shared/login.service';
import { Router } from '@angular/router';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'UCI';
  userIsLogged: boolean;
  checkingUserCredentials: boolean;

  constructor(private readonly loginService: LoginService, private readonly router: Router) {
    this.userIsLogged = false;
    this.checkingUserCredentials = false;

    this.loginService
        .checkingUserCredentials
        .subscribe(() => {
          this.checkingUserCredentials = true;
        });

    this.loginService
        .logginSuccess
        .subscribe((eventResponse: boolean) => {
          this.checkingUserCredentials = false;

          this.userIsLogged = eventResponse;
          if (this.userIsLogged) {
            this.router.navigate(['products']);
          } else {
            Materialize.toast('Credentials are invalid. Check username/password', 3000, 'rounded');
          }
        });
  }
}
