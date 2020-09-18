import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {InventionCategory} from '../shared/invention-category.model';

@Component({
  selector: 'ngx-invention-category-table',
  templateUrl: './invention-category-table.component.html',
  styleUrls: ['./invention-category-table.component.scss']
})
export class InventionCategoryTableComponent implements OnInit {

  @Input()
  inventionCategories: InventionCategory[];

  @Output()
  inventionCategoryDeleteRequested = new EventEmitter<InventionCategory>();

  @Output()
  inventionCategoryEditRequested = new EventEmitter<InventionCategory>();

  settings: any;

  constructor() {
    this.settings = this.getTableSettings();
  }

  ngOnInit() {
  }

  getTableSettings(): any {
    return {
      columns: {
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
    this.inventionCategoryEditRequested.emit(event.data);
  }

  deleteRequested(event): void {
    this.inventionCategoryDeleteRequested.emit(event.data);
  }
}
