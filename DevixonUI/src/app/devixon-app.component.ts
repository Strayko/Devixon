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
export class DevixonAppComponent implements OnInit{
  user: ILoggedUser

  constructor(private userService: UserService) {
    this.userService.currentUser.subscribe(x => this.user = x)
  }

  fbLibrary() {
    (window as any).fbAsyncInit = function() {
      window['FB'].init({
        appId: '2959816677581090',
        cookie: true,
        xfbml: true,
        version: 'v3.1'
      });
      window['FB'].AppEvents.logPageView();
    };
    (function(d, s, id){
      let js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) {return;}
      js = d.createElement(s); js.id = id;
      js.src = "https://connect.facebook.net/en_US/sdk.js";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
  }

  ngOnInit() {
    this.fbLibrary();
  }

  // ngOnInit() {
  //   let token = JSON.parse(`{"name": "${this.user.token}"}`)
  //   this.userService.validate(token).subscribe((data) => {
  //     console.log(data)
  //     return data
  //   })
  // }
}
