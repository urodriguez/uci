import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientsComponent } from './clients.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    ClientsComponent
  ],
  exports: [
    ClientsComponent
  ]
})
export class ClientsModule { }
