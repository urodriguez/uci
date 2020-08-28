import {Component, Inject} from '@angular/core';
import {NB_AUTH_OPTIONS, NbLoginComponent} from '@nebular/auth';
import {LoginStatus} from './login-status.enum';
import {Router} from '@angular/router';
import {AuthService} from '../shared/auth.service';
import {UserCredential} from './user-credential.model';
import {AppContext} from '../../app-context';
import {UserService} from '../../pages/users/shared/user.service';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends NbLoginComponent {
  constructor(@Inject(NB_AUTH_OPTIONS) options = {},
              router: Router,
              private readonly authService: AuthService) {
    super(null, options, null, router);
  }

  login(): void {
    this.errors = [];
    this.messages = [];
    this.submitted = true;

    const userCredential = new UserCredential(this.user.email, this.user.password);
    this.authService.login(userCredential, this.user.rememberMe).subscribe(result => {
      this.submitted = false;

      switch (result.status) {
        case LoginStatus.Success:
          this.messages = ['You have been successfully logged in.'];
          return this.router.navigateByUrl('/');

        case LoginStatus.InvalidEmailOrPassword:
          this.errors = ['Email or Password combination is not correct, please try again.'];
          break;

        case LoginStatus.NonCustomPassword:
          this.messages = ['You have been successfully logged in. New password is required'];
          return this.router.navigateByUrl('auth/reset-password');

        case LoginStatus.Inactive:
          this.errors = ['Your account is inactive.'];
          break;

        case LoginStatus.Locked:
          this.errors = ['Your account is locked'];
          break;

        case LoginStatus.UnconfirmedEmail:
          this.errors = ['Your email account has not been confirmed yet. Please check you email box'];
          break;
      }
    });
  }
}
