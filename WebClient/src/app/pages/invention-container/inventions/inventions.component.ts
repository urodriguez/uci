import { Component } from '@angular/core';

@Component({
  selector: 'ngx-inventions',
  templateUrl: './inventions.component.html',
  styleUrls: ['./inventions.component.scss'],
})
export class InventionsComponent {
  settings: any;
  source: any;

  constructor() {
    this.settings = this.getTableSettings();
    this.source = this.getTableDataSource();
  }

  getTableSettings(): any {
    const settings = {
      add: {
        addButtonContent: '<i class="nb-plus"></i>',
        createButtonContent: '<i class="nb-checkmark"></i>',
        cancelButtonContent: '<i class="nb-close"></i>',
      },
      edit: {
        editButtonContent: '<i class="nb-edit"></i>',
        saveButtonContent: '<i class="nb-checkmark"></i>',
        cancelButtonContent: '<i class="nb-close"></i>',
      },
      delete: {
        deleteButtonContent: '<i class="nb-trash"></i>',
        confirmDelete: true,
      },
      columns: {
        id: {
          title: 'ID',
          type: 'number',
        },
        code: {
          title: 'Code',
          type: 'string',
        },
        name: {
          title: 'Name',
          type: 'string',
        },
        category: {
          title: 'Category',
          type: 'string',
        },
        price: {
          title: 'Price',
          type: 'number',
        },
      },
    };
    return settings;
  }

  getTableDataSource(): any[] {
    return [
      {
        id: 1,
        code: 'code01',
        name: 'invention01',
        category: 'food',
        price: '28',
      },
      {
        id: 2,
        code: 'code02',
        name: 'invention02',
        category: 'other',
        price: '20',
      },
      {
        id: 3,
        code: 'code03',
        name: 'invention03',
        category: 'clothes',
        price: '18',
      },
      {
        id: 4,
        code: 'code04',
        name: 'invention04',
        category: 'technology',
        price: '30',
      },
    ];
  }

  onDeleteConfirm(event): void {
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }
}
