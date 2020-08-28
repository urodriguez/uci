import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from './@core/utils';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-app',
  template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit {

  constructor(private analytics: AnalyticsService,
              private readonly router: Router) {
  }

  ngOnInit(): void {
    this.analytics.trackPageViews();

    if (!localStorage.getItem('securityToken')) {
      this.router.navigateByUrl('/auth/login');
    }
  }
}
