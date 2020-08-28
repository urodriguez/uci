import { NgModule } from '@angular/core';
import {FormsModule} from '@angular/forms';

import {
  NbButtonModule,
  NbCardModule,
  NbCheckboxModule,
  NbDialogModule,
  NbIconModule,
  NbInputModule,
  NbLayoutModule,
  NbSelectModule,
  NbToastrModule
} from '@nebular/theme';

import { ProfileComponent } from './profile.component';
import {ProfileRoutingModule} from './profile-routing.module';

@NgModule({
  declarations: [ProfileComponent],
  imports: [
    NbCardModule,
    NbIconModule,
    NbInputModule,
    NbButtonModule,
    NbDialogModule.forChild(),
    NbCheckboxModule,
    NbSelectModule,
    NbToastrModule.forRoot(),
    NbLayoutModule,
    FormsModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
