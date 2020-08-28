import { Component } from '@angular/core';
import {InventionCategoryService} from './invention-category.service';

@Component({
  selector: 'ngx-invention-categories',
  templateUrl: './invention-categories.component.html',
  styleUrls: ['./invention-categories.component.scss'],
})
export class InventionCategoriesComponent {
  settings: any;
  source: any;

  constructor(private readonly inventionCategoryService: InventionCategoryService) {
    this.settings = this.getTableSettings();
    this.source = inventionCategoryService.getAll();
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
          type: 'string',
        },
        code: {
          title: 'Code',
          type: 'string',
        },
        name: {
          title: 'Name',
          type: 'string',
        },
        description: {
          title: 'Description',
          type: 'string',
        },
      },
    };
    return settings;
  }

  onDeleteConfirm(event): void {
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }
}
