import {Component, Input} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {InventionCategory} from '../shared/invention-category.model';

@Component({
  selector: 'ngx-invention-category-create-or-update',
  templateUrl: './invention-category-create-or-update.component.html',
  styleUrls: ['./invention-category-create-or-update.component.scss']
})
export class InventionCategoryCreateOrUpdateComponent {
  @Input()
  inventionCategory: InventionCategory;

  constructor(private readonly dialogRef: NbDialogRef<InventionCategoryCreateOrUpdateComponent>) {}

  cancel() {
    this.dialogRef.close(false);
  }

  save() {
    this.dialogRef.close(true);
  }
}

