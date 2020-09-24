import {Component, EventEmitter, Input, Output} from '@angular/core';
import {User} from '../shared/user.model';

@Component({
  selector: 'ngx-user-table',
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss']
})
export class UserTableComponent {
  @Input()
  source: User[];

  @Output()
  onOpenUpdateView = new EventEmitter<User>();

  @Output()
  onOpenDeleteView = new EventEmitter<User>();

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
        roleName: {
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

  openUpdateView(event: any): void {
    this.onOpenUpdateView.emit(event.data);
  }

  openDeleteView(event: any): void {
    this.onOpenDeleteView.emit(event.data);
  }
}
