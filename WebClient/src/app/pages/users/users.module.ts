import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  NbButtonModule,
  NbCardModule,
  NbCheckboxModule,
  NbDialogModule,
  NbIconModule,
  NbInputModule,
  NbSelectModule,
  NbToastrModule
} from '@nebular/theme';

import { ThemeModule } from '../../@theme/theme.module';

import { Ng2SmartTableModule } from 'ng2-smart-table';

import { UsersComponent } from './users.component';
import {UserCreateOrUpdateComponent} from './user-create-or-update/user-create-or-update.component';
import { UserTableComponent } from './user-table/user-table.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';
import {UsersRoutingModule} from './users-routing.module';

@NgModule({
  imports: [
    NbCardModule,
    NbIconModule,
    NbInputModule,
    NbButtonModule,
    NbDialogModule.forChild(),
    NbCheckboxModule,
    NbSelectModule,
    NbToastrModule.forRoot(),
    ThemeModule,
    Ng2SmartTableModule,
    FormsModule,
    UsersRoutingModule
  ],
  declarations: [
    UsersComponent,
    UserTableComponent,
    UserCreateOrUpdateComponent,
    UserDeleteComponent
  ],
  entryComponents: [
    UserCreateOrUpdateComponent,
    UserDeleteComponent
  ],
})
export class UsersModule { }
