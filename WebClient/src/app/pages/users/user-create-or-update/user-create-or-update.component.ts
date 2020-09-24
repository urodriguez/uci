import {Component, Input} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {User} from '../shared/user.model';

@Component({
  selector: 'ngx-user-create-or-update',
  templateUrl: 'user-create-or-update.component.html',
  styleUrls: ['user-create-or-update.component.scss'],
})
export class UserCreateOrUpdateComponent {
  @Input()
  model: User;

  constructor(private readonly dialogRef: NbDialogRef<UserCreateOrUpdateComponent>) {}

  cancel() {
    this.dialogRef.close(false);
  }

  save() {
    this.dialogRef.close(true);
  }
}
