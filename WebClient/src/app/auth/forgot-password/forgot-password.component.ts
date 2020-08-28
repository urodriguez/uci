import {Component, Inject} from '@angular/core';
import {NB_AUTH_OPTIONS, NbRequestPasswordComponent} from '@nebular/auth';
import {Router} from '@angular/router';
import {UserService} from '../../pages/users/shared/user.service';

@Component({
  selector: 'ngx-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent extends NbRequestPasswordComponent {
  constructor(@Inject(NB_AUTH_OPTIONS) options = {},
              router: Router,
              private readonly userService: UserService) {
    super(null, options, null, router);
  }

  forgotPassword() {
    this.errors = this.messages = [];
    this.submitted = true;

    this.userService.forgotPassword(this.user.email).subscribe(
      result => {
        this.submitted = false;
        this.messages = ['Reset password instructions have been sent to your email.'];
      },
      error => {
        this.submitted = false;
        this.errors = [error];
      }
    );
  }
}
