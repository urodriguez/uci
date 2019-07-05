import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProductsComponent } from './products/products.component';
import { LoginComponent } from './login/login.component';
import { ClientsComponent } from './clients/clients.component';

const routes: Routes = [
/*     {
        path: 'products',
        loadChildren: () => import('./products/products.module').then(mod => mod.ProductsModule)
    },
    {
        path: 'clients',
        loadChildren: () => import('./clients/clients.module').then(mod => mod.ClientsModule)
    },
    {
        path: '',
        redirectTo: '',
        pathMatch: 'full'
    } */
    { path: '', redirectTo: '', pathMatch: 'full' },
    /* { path: 'login', component: LoginComponent }, */
    { path: 'products', component: ProductsComponent },
    { path: 'clients', component: ClientsComponent },
];

@NgModule({
    imports: [
      RouterModule.forRoot(routes)
    ],
    exports: [ RouterModule ]
  })
export class AppRoutingModule {
}
