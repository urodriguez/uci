import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent {
  showCollapseButton: boolean;

  @Input()
  set userIsLogged(userIsLogged: boolean) {
    this.showCollapseButton = userIsLogged;

    if (this.showCollapseButton) {
      $('#btn-collapse').show();
      $('#btn-collapse').sideNav({
        menuWidth: 300, // Default is 300
        edge: 'left', // Choose the horizontal origin
        closeOnClick: true, // Closes side-nav on <a> clicks, useful for Angular/Meteor
        draggable: true // Choose whether you can drag to open on touch screens
      });
    } else {
      $('#btn-collapse').hide();
    }
  }

  constructor() { }
}
