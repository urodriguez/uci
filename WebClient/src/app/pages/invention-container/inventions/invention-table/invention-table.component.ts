import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Invention} from '../shared/invention.model';

@Component({
  selector: 'ngx-invention-table',
  templateUrl: './invention-table.component.html',
  styleUrls: ['./invention-table.component.scss']
})
export class InventionTableComponent {
  @Input()
  source: Invention[];

  @Output()
  onOpenStateView = new EventEmitter<Invention>();

  settings: any;

  constructor() {
    this.settings = this.getTableSettings();
  }

  getTableSettings(): any {
    return {
      columns: {
        code: {
          title: 'Code',
          type: 'string',
        },
        inventorName: {
          title: 'Inventor Name',
          type: 'string',
        },
        description: {
          title: 'Description',
          type: 'string',
        },
        categoryName: {
          title: 'Category',
          type: 'string',
        },
        price: {
          title: 'Price',
          type: 'number',
        },
        enable: {
          title: 'Enable?',
          type: 'boolean',
        },
      },
      hideSubHeader: true,
      mode: 'external', //avoid inline edit
      actions: {
        add: false,
        edit: false,
        position: 'right'
      },
      delete: {
        deleteButtonContent: '<i class="nb-power-circled"></i>',
        confirmDelete: true,
      },
    };
  }

  openStateView(event: any): void {
    this.onOpenStateView.emit(event.data);
  }
}
