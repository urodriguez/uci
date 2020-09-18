import {Component, Input} from '@angular/core';
import {InventionCategory} from '../shared/invention-category.model';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-invention-category-delete',
  templateUrl: './invention-category-delete.component.html',
  styleUrls: ['./invention-category-delete.component.scss']
})
export class InventionCategoryDeleteComponent {
  @Input()
  inventionCategory: InventionCategory;

  constructor(private readonly dialogRef: NbDialogRef<InventionCategoryDeleteComponent>) { }

  cancel() {
    this.dialogRef.close(false);
  }

  delete() {
    this.dialogRef.close(true);
  }
}

