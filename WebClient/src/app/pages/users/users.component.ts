import {Component} from '@angular/core';

import {NbDialogService, NbToastrService} from '@nebular/theme';

import {UserService} from './shared/user.service';
import {UserCreateOrUpdateComponent} from './user-create-or-update/user-create-or-update.component';
import {User} from './shared/user.model';
import {UserDeleteComponent} from './user-delete/user-delete.component';

@Component({
  selector: 'ngx-users',
  styleUrls: ['./users.component.scss'],
  templateUrl: './users.component.html',
})
export class UsersComponent {

  users: User[];

  moduleName: string;
  modelName: string;

  constructor(private readonly modelService: UserService,
              private readonly dialogService: NbDialogService,
              private readonly toastrService: NbToastrService) {
    this.modelService.getAll().subscribe(
      result => this.users = result,
      errorMessage => this.showErrorToaster(errorMessage)
    );

    this.moduleName = 'Users';
    this.modelName = User.name;
  }

  openDeleteView(model: User) {
    const dialogRef = this.dialogService.open(UserDeleteComponent, {
      context: {
        model: model,
      },
    });

    dialogRef.onClose.subscribe((deleteModel: boolean) => {
      if (deleteModel) {
        this.toastrService.show('', `Deleting ${this.modelName}`, { status: 'info' });
        this.modelService.delete(model).subscribe(
          () => {
            this.users = this.users.filter(u => u.id !== model.id);
            this.toastrService.show('', `${this.modelName} Deleted`, { status: 'success' });
          },
          errorMessage => this.showErrorToaster(errorMessage)
        );
      }
    });
  }

  openCreateOrUpdateView(model: User = new User()) {
    const dialogRef = this.dialogService.open(UserCreateOrUpdateComponent, {
      context: {
        model: model
      }
    });

    dialogRef.onClose.subscribe((saveModel: boolean) => {
      if (saveModel) {
        if (model.id == null) {
          this.toastrService.show('', `Creating ${this.modelName}`, { status: 'info' });
          this.modelService.create(model).subscribe(
            id => {
              model.id = id;
              this.users = [...this.users, model];
              this.toastrService.show('', `${this.modelName} Created`, { status: 'success' });
            },
            errorMessage => {
              this.showErrorToaster(errorMessage);
              this.openCreateOrUpdateView(model); //reopens closed dialog
            }
          );
        } else {
          this.toastrService.show('', `Updating ${this.modelName}`, { status: 'info' });
          this.modelService.update(model).subscribe(
            () => {
              this.users = this.users.map(u => {
                if (u.id === model.id) return model;
                else return u;
              });
              this.toastrService.show('', `${this.modelName} Updated`, { status: 'success' });
            },
            errorMessage => {
              this.showErrorToaster(errorMessage);
              this.openCreateOrUpdateView(model); //reopens closed dialog
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
