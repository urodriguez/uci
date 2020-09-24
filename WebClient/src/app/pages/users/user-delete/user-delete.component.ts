import {Component, Input} from '@angular/core';
import {User} from '../shared/user.model';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.scss']
})
export class UserDeleteComponent {
  @Input()
  model: User;

  constructor(private readonly dialogRef: NbDialogRef<UserDeleteComponent>) { }

  cancel() {
    this.dialogRef.close(false);
  }

  delete() {
    this.dialogRef.close(true);
  }
}
