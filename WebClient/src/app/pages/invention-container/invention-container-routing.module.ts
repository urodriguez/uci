import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventionContainerComponent } from './invention-container.component';
import { InventionsComponent } from './inventions/inventions.component';
import { InventionCategoriesComponent } from './invention-categories/invention-categories.component';

const routes: Routes = [{
  path: '',
  component: InventionContainerComponent,
  children: [
    {
      path: 'inventions',
      component: InventionsComponent,
    },
    {
      path: 'categories',
      component: InventionCategoriesComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventionContainerRoutingModule { }

export const routedComponents = [
  InventionContainerComponent,
  InventionsComponent,
  InventionCategoriesComponent,
];
