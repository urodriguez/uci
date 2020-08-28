import {Component, OnChanges, OnDestroy, OnInit} from '@angular/core';
import { NbMediaBreakpointsService, NbMenuService, NbSidebarService, NbThemeService } from '@nebular/theme';

import { LayoutService } from '../../../@core/utils';
import { map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import {User} from '../../../pages/users/shared/user.model';
import {AuthService} from '../../../auth/shared/auth.service';
import {AppContext} from '../../../app-context';
import {Router} from '@angular/router';
import {NbMenuBag} from '@nebular/theme/components/menu/menu.service';

@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit, OnDestroy {

  private destroy$: Subject<void> = new Subject<void>();
  userPictureOnly: boolean = false;
  user: User;

  currentTheme = 'default';

  userMenu = [
    {
      title: 'Profile',
      icon: 'person-outline',
    },
    {
      title: 'Reset Password',
      icon: 'lock-outline',
    },
    {
      title: 'Privacy Policy',
      icon: 'shield-outline',
    },
    {
      title: 'Log out',
      icon: 'unlock-outline',
    }
  ];

  constructor(private sidebarService: NbSidebarService,
              private menuService: NbMenuService,
              private themeService: NbThemeService,
              private layoutService: LayoutService,
              private breakpointService: NbMediaBreakpointsService,
              private readonly appContext: AppContext,
              private readonly authService: AuthService,
              private readonly router: Router) {
    this.appContext.loggedUserChanged.subscribe(lu => this.user = lu);
  }

  ngOnInit() {
    this.currentTheme = this.themeService.currentTheme;

    this.menuService.onItemClick().subscribe((menuBag: NbMenuBag) => {
      if (menuBag && menuBag.item.title === 'Log out'){
        this.authService.logout();
        return this.router.navigateByUrl('/auth/login');
      }

      if (menuBag && menuBag.item.title === 'Privacy Policy'){
        return this.router.navigateByUrl('/pages/private-policy');
      }

      if (menuBag && menuBag.item.title === 'Profile'){
        return this.router.navigateByUrl('/pages/profile');
      }

      if (menuBag && menuBag.item.title === 'Reset Password'){
        return this.router.navigateByUrl('/auth/reset-password');
      }
    });

    const { xl } = this.breakpointService.getBreakpointsMap();
    this.themeService.onMediaQueryChange()
      .pipe(
        map(([, currentBreakpoint]) => currentBreakpoint.width < xl),
        takeUntil(this.destroy$),
      )
      .subscribe((isLessThanXl: boolean) => this.userPictureOnly = isLessThanXl);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  navigateHome() {
    this.menuService.navigateHome();
    return false;
  }
}
