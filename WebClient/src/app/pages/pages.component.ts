import {Component, OnInit} from '@angular/core';

import {NbMenuItem} from '@nebular/theme';
import {UserService} from './users/shared/user.service';
import {AppContext} from '../app-context';

@Component({
  selector: 'ngx-pages',
  styleUrls: ['pages.component.scss'],
  template: `
    <ngx-one-column-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-one-column-layout>
  `,
})
export class PagesComponent implements OnInit {
  menu: NbMenuItem[];

  constructor(private readonly userService: UserService,
              private readonly appContext: AppContext) {
    if(!this.appContext.securityToken)
      this.appContext.securityToken = JSON.parse(localStorage.getItem('securityToken'));
  }

  ngOnInit(): void {
    this.userService.getLogged().subscribe(lu => this.appContext.loggedUser = lu);

    this.menu= [
      {
        title: 'E-commerce',
        icon: 'shopping-cart-outline',
        link: '/pages/dashboard',
        home: true,
      },
      {
        title: 'Users',
        icon: 'person-outline',
        link: '/pages/users',
      },
      {
        title: 'Invention',
        icon: 'bulb-outline',
        children: [
          {
            title: 'Inventions',
            link: '/pages/invention-container/inventions',
          },
          {
            title: 'Categories',
            link: '/pages/invention-container/categories',
          },
        ],
      }
    ];
  }
}
