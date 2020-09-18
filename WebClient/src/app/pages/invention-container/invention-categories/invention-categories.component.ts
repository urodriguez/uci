import { Component } from '@angular/core';
import {InventionCategoryService} from './shared/invention-category.service';
import {InventionCategory} from './shared/invention-category.model';
import {NbDialogService, NbToastrService} from '@nebular/theme';
import {InventionCategoryDeleteComponent} from './invention-category-delete/invention-category-delete.component';
import {
  InventionCategoryCreateOrUpdateComponent
} from './invention-category-create-or-update/invention-category-create-or-update.component';

@Component({
  selector: 'ngx-invention-categories',
  templateUrl: './invention-categories.component.html',
  styleUrls: ['./invention-categories.component.scss'],
})
export class InventionCategoriesComponent {
  inventionCategories: InventionCategory[];

  constructor(private readonly inventionCategoryService: InventionCategoryService,
              private readonly dialogService: NbDialogService,
              private readonly toastrService: NbToastrService) {
    this.inventionCategoryService.getAll().subscribe(
      inventionCategories => this.inventionCategories = inventionCategories,
      errorMessage => this.showErrorToaster(errorMessage)
    );
  }

  orchestrateInventionCategoryCreateRequested() {
    this.openCreateOrUpdateDialog(new InventionCategory());
  }

  orchestrateInventionCategoryEditRequested(inventionCategory: InventionCategory) {
    this.openCreateOrUpdateDialog(inventionCategory);
  }

  orchestrateInventionCategoryDeleteRequested(inventionCategory: InventionCategory) {
    const dialogRef = this.dialogService.open(InventionCategoryDeleteComponent, {
      context: {
        inventionCategory: inventionCategory,
      },
    });

    dialogRef.onClose.subscribe((deleteInventionCategory: boolean) => {
      if (deleteInventionCategory) {
        this.toastrService.show('', `Deleting Invention Category`, { status: 'info' });
        this.inventionCategoryService.delete(inventionCategory).subscribe(
          () => {
            this.inventionCategories = this.inventionCategories.filter(u => u.id !== inventionCategory.id);
            this.toastrService.show('', `Invention Category Deleted`, { status: 'success' });
          },
          errorMessage => this.showErrorToaster(errorMessage)
        );
      }
    });
  }

  private openCreateOrUpdateDialog(inventionCategory: InventionCategory) {
    const dialogRef = this.dialogService.open(InventionCategoryCreateOrUpdateComponent, {
      context: {
        inventionCategory: inventionCategory,
      },
    });

    dialogRef.onClose.subscribe((saveUser: boolean) => {
      if (saveUser) {
        if (inventionCategory.id == null) {
          this.toastrService.show('', `Creating Invention Category`, { status: 'info' });
          this.inventionCategoryService.create(inventionCategory).subscribe(
            id => {
              inventionCategory.id = id;
              this.inventionCategories = [...this.inventionCategories, inventionCategory];
              this.toastrService.show('', `Invention Category Created`, { status: 'success' });
            },
            errorMessage => {
              this.showErrorToaster(errorMessage);
              this.openCreateOrUpdateDialog(inventionCategory); //reopens closed dialog
            }
          );
        } else {
          this.toastrService.show('', `Updating Invention Category`, { status: 'info' });
          this.inventionCategoryService.update(inventionCategory).subscribe(
            () => {
              this.inventionCategories = this.inventionCategories.map(u => {
                if (u.id === inventionCategory.id) return inventionCategory;
                else return u;
              });
              this.toastrService.show('', `Invention Category Updated`, { status: 'success' });
            },
            errorMessage => {
              this.showErrorToaster(errorMessage);
              this.openCreateOrUpdateDialog(inventionCategory); //reopens closed dialog
            }
          );
        }
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
