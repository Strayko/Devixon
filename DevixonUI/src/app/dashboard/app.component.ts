import { Component } from '@angular/core';
import {UserService} from '../shared/user.service';
import {Router} from '@angular/router';
import {ILoggedUser} from '../_interface/logged-user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  user: ILoggedUser

  constructor(private userService: UserService, private router: Router) {
    this.userService.currentUser.subscribe(x=>this.user = x)
  }

  isAuthenticated() {
    return !!this.user
  }

  login() {
    window['FB'].login((response) => {
      let token = JSON.parse(`{"FacebookToken": "${response.authResponse.accessToken}"}`)
      this.userService.loginFacebook(token).subscribe((data) => {
        if (data != null) {
          location.reload(true)
          this.router.navigate(['/'])
        }
      }, (error) => {
        console.log('User login failed: ' + error);
      })
      // if (response.authResponse) {
      //   window['FB'].api('/me', {
      //     fields: 'last_name, first_name, email'
      //   }, (userInfo) => {
      //
      //     console.log("user information");
      //     console.log(userInfo);
      //   });
      //
      // } else {
      //   console.log('User login failed');
      // }
    }, {scope: 'email'})
  }
}
