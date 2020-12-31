import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {UserService} from '../shared/user.service';

@Injectable({providedIn: 'root'})
export class LoggedGuard implements CanActivate {
  constructor(private router: Router, private userService: UserService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this.userService.currentUserValue
    if (currentUser) {
      this.router.navigate(['/'], {queryParams: {returnUrl: state.url}})
      return false
    }
    return true
  }
}
