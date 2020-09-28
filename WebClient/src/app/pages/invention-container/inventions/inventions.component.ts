import {Component} from '@angular/core';

import {NbDialogService, NbToastrService} from '@nebular/theme';

import {InventionService} from './shared/invention.service';
import {Invention} from './shared/invention.model';
import {InventionStateComponent} from './invention-state/invention-state.component';

@Component({
  selector: 'ngx-inventions',
  styleUrls: ['./inventions.component.scss'],
  templateUrl: './inventions.component.html',
})
export class InventionsComponent {

  inventions: Invention[];

  moduleName: string;
  modelName: string;

  constructor(private readonly modelService: InventionService,
              private readonly dialogService: NbDialogService,
              private readonly toastrService: NbToastrService) {
    this.modelService.getAll().subscribe(
      result => this.inventions = result,
      errorMessage => this.showErrorToaster(errorMessage)
    );

    this.moduleName = 'Inventions';
    this.modelName = Invention.name;
  }

  openStateView(model: Invention) {
    const dialogRef = this.dialogService.open(InventionStateComponent, {
      context: {
        model: model,
      },
    });

    dialogRef.onClose.subscribe((changeState: boolean) => {
      if (changeState) {
        this.toastrService.show('', `Changing state of ${this.modelName}`, { status: 'info' });
        model.enable = !model.enable;
        this.modelService.updateState(model).subscribe(
          () => {
            this.inventions = this.inventions.map(i => i);
            this.toastrService.show('', `${this.modelName} state changed`, { status: 'success' });
          },
          errorMessage => this.showErrorToaster(errorMessage)
        );
      }
    });
  }

  private showErrorToaster(errorMessage: string): void {
    this.toastrService.show(
      errorMessage,
      `An error has occurred`,
      { status: 'danger', duration: 8000, destroyByClick: false }
    );
  }
}
