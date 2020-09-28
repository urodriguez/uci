import {Component, Input} from '@angular/core';
import {Invention} from '../shared/invention.model';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-invention-disable',
  templateUrl: './invention-state.component.html',
  styleUrls: ['./invention-state.component.scss']
})
export class InventionStateComponent {
  @Input()
  model: Invention;

  constructor(private readonly dialogRef: NbDialogRef<InventionStateComponent>) { }

  cancel() {
    this.dialogRef.close(false);
  }

  delete() {
    this.dialogRef.close(true);
  }
}
