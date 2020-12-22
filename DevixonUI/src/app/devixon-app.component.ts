import {Component, OnInit} from '@angular/core';
import {User} from './shared/user-details';
import {UserService} from './shared/user.service';
import {HTTP_INTERCEPTORS} from '@angular/common/http';

@Component({
  selector: 'devixon-app',
  template: `
    <nav-bar></nav-bar>
    <router-outlet></router-outlet>
  `
})
export class DevixonAppComponent{
  user: User

  constructor(private userService: UserService) {
    this.userService.currentUser.subscribe(x => this.user = x)
  }
}
