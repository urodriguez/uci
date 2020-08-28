import {Component, Inject} from '@angular/core';
import {NB_AUTH_OPTIONS, NbResetPasswordComponent} from '@nebular/auth';
import {Router} from '@angular/router';
import {UserService} from '../../pages/users/shared/user.service';
import {ResetPassword} from './reset-password.model';

@Component({
  selector: 'ngx-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent extends NbResetPasswordComponent {
  constructor(@Inject(NB_AUTH_OPTIONS) options = {},
              router: Router,
              private readonly userService: UserService) {
    super(null, options, null, router);
  }

  resetPass(): void {
    this.errors = this.messages = [];
    this.submitted = true;

    const resetPassword = new ResetPassword(this.user.oldPassword, this.user.password);
    this.userService.resetPassword(resetPassword).subscribe(
      result => {
        this.submitted = false;
        this.messages = ['Your password has been successfully reset'];
        return this.router.navigateByUrl('/');
      },
      error => {
        this.submitted = false;
        this.errors = [error];
      }
    );
  }
}
