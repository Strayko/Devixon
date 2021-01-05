import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {UserService} from '../shared/user.service';
import {ILoggedUser} from '../_interface/logged-user';

@Component({
  selector: 'nav-bar',
  templateUrl: './nav.component.html'
})
export class NavComponent{
  user: ILoggedUser
  hiddenToggle = true

  constructor(private userService: UserService) {
    this.userService.currentUser.subscribe(x => this.user = x)
  }

  isAuthenticated() {
    return !!this.user
  }

  toggleClassHidden() {
    this.hiddenToggle = !this.hiddenToggle
  }

  logout() {
    this.userService.logout()
  }
}
