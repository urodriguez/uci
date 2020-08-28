import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';
import { ECommerceComponent } from './e-commerce/e-commerce.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'dashboard',
      component: ECommerceComponent,
    },
    {
      path: 'users',
      loadChildren: () => import('./users/users.module').then(m => m.UsersModule),
    },
    {
      path: 'profile',
      loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule),
    },
    {
      path: 'invention-container',
      loadChildren: () => import('./invention-container/invention-container.module').then(m => m.InventionContainerModule),
    },
    {
      path: '',
      redirectTo: 'dashboard',
      pathMatch: 'full',
    },
    {
      path: '**',
      redirectTo: 'dashboard',
      pathMatch: 'full',
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
