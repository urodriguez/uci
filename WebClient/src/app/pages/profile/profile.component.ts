import { Component, OnInit } from '@angular/core';
import {User} from '../users/shared/user.model';
import {AppContext} from '../../app-context';
import {UserService} from '../users/shared/user.service';
import {NbToastrService} from '@nebular/theme';
import {UserFactory} from '../users/shared/user.factory';

@Component({
  selector: 'ngx-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: User;

  constructor(private readonly appContext: AppContext,
              private readonly toastrService: NbToastrService,
              private readonly userService: UserService,
              private readonly userFactory: UserFactory) {
  }

  ngOnInit() {
    //creates a copy
    this.user = this.userFactory.create(this.appContext.loggedUser);
  }

  save() {
    this.toastrService.show('', `Updating Profile`, { status: 'info' });
    this.userService.update(this.user).subscribe(
      () => {
        this.toastrService.show('',`Profile Updated`,{ status: 'success' });
        this.appContext.loggedUser = this.user;
      },
      errorMessage => {
        this.toastrService.show(
          errorMessage,
          `An error has occurred`,
          { status: 'danger', duration: 8000, destroyByClick: false }
        );
      }
    );
  }
}
