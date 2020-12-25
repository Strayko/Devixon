import {Component, OnInit} from '@angular/core';
import {UserService} from './shared/user.service';
import {ILoggedUser} from './_interface/logged-user';

@Component({
  selector: 'devixon-app',
  template: `
    <nav-bar></nav-bar>
    <router-outlet></router-outlet>
  `
})
export class DevixonAppComponent{
  user: ILoggedUser

  constructor(private userService: UserService) {
    this.userService.currentUser.subscribe(x => this.user = x)
  }

  // ngOnInit() {
  //   let token = JSON.parse(`{"name": "${this.user.token}"}`)
  //   this.userService.validate(token).subscribe((data) => {
  //     console.log(data)
  //     return data
  //   })
  // }
}
