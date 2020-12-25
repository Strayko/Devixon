import {Component, OnInit} from '@angular/core';
import {UserService} from '../shared/user.service';
import {IUser} from '../_interface/user-details';
import { OperatorFunction } from 'rxjs';

@Component({
  templateUrl: './details.component.html'
})
export class DetailsComponent implements OnInit{
  public user: IUser | OperatorFunction<unknown, unknown>

  constructor(private userService: UserService) {
  }

  ngOnInit() {
    this.userService.details().subscribe( (data) => {
      this.user = data
    })
  }

}
