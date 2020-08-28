import {Component, EventEmitter, Input, Output} from '@angular/core';
import {User} from '../shared/user.model';

@Component({
  selector: 'ngx-user-table',
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss']
})
export class UserTableComponent {
  @Input()
  users: User[];

  @Output()
  userDeleteRequested = new EventEmitter<User>();

  @Output()
  userEditRequested = new EventEmitter<User>();

  settings: any;

  constructor() {
    this.settings = this.getTableSettings();
  }

  getTableSettings(): any {
    return {
      columns: {
        email: {
          title: 'Email',
          type: 'string',
        },
        firstName: {
          title: 'First Name',
          type: 'string',
        },
        lastName: {
          title: 'Last Name',
          type: 'string',
        },
        role: {
          title: 'Role',
          type: 'string',
        },
      },
      hideSubHeader: true,
      mode: 'external', //avoid inline edit
      actions: {
        add: false,
        position: 'right'
      },
      edit: {
        editButtonContent: '<i class="nb-edit"></i>',
      },
      delete: {
        deleteButtonContent: '<i class="nb-trash"></i>',
        confirmDelete: true,
      },
    };
  }

  editRequested(event: any): void {
    this.userEditRequested.emit(event.data);
  }

  deleteRequested(event): void {
    this.userDeleteRequested.emit(event.data);
  }
}
