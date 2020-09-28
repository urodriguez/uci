import { NgModule } from '@angular/core';
import {FormsModule} from '@angular/forms';

import {
  NbButtonModule,
  NbCardModule,
  NbDialogModule,
  NbIconModule,
  NbInputModule,
  NbSelectModule,
  NbToastrModule
} from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';

import { ThemeModule } from '../../@theme/theme.module';

import { InventionContainerRoutingModule } from './invention-container-routing.module';
import {InventionContainerComponent} from './invention-container.component';

import {InventionsComponent} from './inventions/inventions.component';
import {InventionTableComponent} from './inventions/invention-table/invention-table.component';
import {InventionStateComponent} from './inventions/invention-state/invention-state.component';

import {
  InventionCategoryTableComponent
} from './invention-categories/invention-category-table/invention-category-table.component';
import {
  InventionCategoriesComponent
} from './invention-categories/invention-categories.component';
import {
  InventionCategoryDeleteComponent
} from './invention-categories/invention-category-delete/invention-category-delete.component';
import {
  InventionCategoryCreateOrUpdateComponent
} from './invention-categories/invention-category-create-or-update/invention-category-create-or-update.component';


@NgModule({
  imports: [
    NbCardModule,
    NbIconModule,
    NbInputModule,
    NbButtonModule,
    NbDialogModule.forChild(),
    NbSelectModule,
    NbToastrModule.forRoot(),
    ThemeModule,
    Ng2SmartTableModule,
    FormsModule,
    InventionContainerRoutingModule,
  ],
  declarations: [
    InventionContainerComponent,
    InventionsComponent,
    InventionTableComponent,
    InventionStateComponent,
    InventionCategoriesComponent,
    InventionCategoryTableComponent,
    InventionCategoryDeleteComponent,
    InventionCategoryCreateOrUpdateComponent
  ],
  entryComponents: [
    InventionStateComponent,
    InventionCategoryDeleteComponent,
    InventionCategoryCreateOrUpdateComponent
  ]
})
export class InventionContainerModule { }
