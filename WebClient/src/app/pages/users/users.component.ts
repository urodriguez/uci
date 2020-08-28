import {Component} from '@angular/core';

import {NbDialogService, NbToastrService} from '@nebular/theme';

import {UserService} from './shared/user.service';
import {UserCreateOrUpdateComponent} from './user-create-or-update/user-create-or-update.component';
import {User} from './shared/user.model';
import {UserDeleteComponent} from './user-delete/user-delete.component';
import {NbAuthJWTToken, NbAuthService} from '@nebular/auth';

@Component({
  selector: 'ngx-users',
  styleUrls: ['./users.component.scss'],
  templateUrl: './users.component.html',
})
export class UsersComponent {

  users: User[];

  constructor(private readonly userService: UserService,
              private readonly dialogService: NbDialogService,
              private readonly toastrService: NbToastrService) {
    this.userService.getAll().subscribe(
      users => this.users = users,
      errorMessage => this.showErrorToaster(errorMessage)
    );
  }

  orchestrateUserCreateRequested() {
    this.openCreateOrUpdateDialog(new User());
  }

  orchestrateUserEditRequested(user: User) {
    this.openCreateOrUpdateDialog(user);
  }

  orchestrateUserDeleteRequested(user: User) {
    const dialogRef = this.dialogService.open(UserDeleteComponent, {
      context: {
        user: user,
      },
    });

    dialogRef.onClose.subscribe((deleteUser: boolean) => {
      if (deleteUser) {
        this.toastrService.show('', `Deleting User`, { status: 'info' });
        this.userService.delete(user).subscribe(
          () => {
            this.users = this.users.filter(u => u.id !== user.id);
            this.toastrService.show('', `User Deleted`, { status: 'success' });
          },
          errorMessage => this.showErrorToaster(errorMessage)
        );
      }
    });
  }

  private openCreateOrUpdateDialog(user: User) {
    const dialogRef = this.dialogService.open(UserCreateOrUpdateComponent, {
      context: {
        user: user,
      },
    });

    dialogRef.onClose.subscribe((saveUser: boolean) => {
      if (saveUser) {
        if (user.id == null) {
          this.toastrService.show('', `Creating User`, { status: 'info' });
          this.userService.create(user).subscribe(
            id => {
              user.id = id;
              this.users = [...this.users, user];
              this.toastrService.show('', `User Created`, { status: 'success' });
            },
            errorMessage => {
              this.showErrorToaster(errorMessage);
              this.openCreateOrUpdateDialog(user); //reopens closed dialog
            }
          );
        } else {
          this.toastrService.show('', `Updating User`, { status: 'info' });
          this.userService.update(user).subscribe(
            () => {
              this.users = this.users.map(u => {
                if (u.id === user.id) return user;
                else return u;
              });
              this.toastrService.show('', `User Updated`, { status: 'success' });
            },
            errorMessage => {
              this.showErrorToaster(errorMessage);
              this.openCreateOrUpdateDialog(user); //reopens closed dialog
            }
          );
        }
      }
    });
  }

  private showErrorToaster(errorMessage: string): void {
    this.toastrService.show(
      errorMessage,
      `An error has occurred`,
      { status: 'danger', duration: 8000, destroyByClick: false }
    );
  }
}
