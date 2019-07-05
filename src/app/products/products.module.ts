import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ProductsComponent } from './products.component';
import { AgGridModule } from 'ag-grid-angular';

@NgModule({
  imports: [
    CommonModule,
    AgGridModule.withComponents([]),
    FormsModule,
    HttpClientModule
  ],
  declarations: [
    ProductsComponent
  ],
  exports: [
    ProductsComponent
  ]
})
export class ProductsModule { }
