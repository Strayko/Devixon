import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {UserService} from '../shared/user.service';
import {Router} from '@angular/router';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  constructor(private userService: UserService, private router: Router) {
  }

  invalidCredentials: boolean
  loginForm: FormGroup
  private email: FormControl
  private password: FormControl

  ngOnInit() {
    this.email = new FormControl(this.email, [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]);
    this.password = new FormControl(this.password, Validators.required);
    this.loginForm = new FormGroup({
      email: this.email,
      password: this.password
    })
  }

  validateEmail() {
    return this.email.valid || this.email.untouched
  }

  validatePassword() {
    return this.password.valid || this.password.untouched
  }

  isEmpty(userForm) {
    let empty = userForm.email
    return (empty == null);
  }

  loginUser(formValues) {
    if (formValues.valid) {
      let objectValue = formValues.value
      this.userService.login(objectValue).subscribe((data) => {
        if (data != null) {
          this.router.navigate(['/'])
        }
      }, (error) => {
        if (error == 401) {
          this.invalidCredentials = true
        }
      })
    }
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsTouched({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }
}
