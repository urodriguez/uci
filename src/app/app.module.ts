import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MzButtonModule, MzInputModule } from 'ngx-materialize';

import { ProductsModule } from './products/products.module';
import { AppRoutingModule } from './app-routing.module';
import { LoginModule } from './login/login.module';
import { ClientsModule } from './clients/clients.module';

import { AppComponent } from './app.component';
import { MainContentComponent } from './main-content/main-content.component';
import { FooterBarComponent } from './footer-bar/footer-bar.component';
import { TopBarComponent } from './top-bar/top-bar.component';

@NgModule({
   declarations: [
      AppComponent,
      MainContentComponent,
      FooterBarComponent,
      TopBarComponent,
   ],
   imports: [
      BrowserModule,
      ProductsModule,
      MzButtonModule,
      MzInputModule,
      AppRoutingModule,
      LoginModule,
      ClientsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
