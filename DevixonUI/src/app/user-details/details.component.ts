import {Component, OnInit} from '@angular/core';
import {UserService} from '../shared/user.service';

@Component({
  templateUrl: './details.component.html'
})
export class DetailsComponent implements OnInit{
  user: Object

  constructor(private userService: UserService) {
  }

  ngOnInit() {
    this.userService.details().subscribe( (data) => {
      this.user = data
      console.log(this.user)
    })
  }

}
