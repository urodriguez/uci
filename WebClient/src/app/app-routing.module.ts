import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('app/auth/auth.module').then(m => m.AuthModule),
  },
  {
    path: 'pages',
    loadChildren: () => import('app/pages/pages.module').then(m => m.PagesModule),
  },
  { path: '', redirectTo: 'pages', pathMatch: 'full' },
  { path: '**', redirectTo: 'pages' },
];

const extraOptions: ExtraOptions = {
  useHash: false,
};

@NgModule({
  imports: [RouterModule.forRoot(routes, extraOptions)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
