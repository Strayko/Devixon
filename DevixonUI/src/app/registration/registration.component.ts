import {Component, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {UserService} from '../shared/user.service';
import {Router} from '@angular/router';

@Component({
  templateUrl: './registration.component.html',
  styles: [`
    em {
      float: right;
      color: #E05C65;
      padding-left: 10px;
    }
    .error input {
      background-color: #E3C3C5;
    }
    .error ::-webkit-input-placeholder {color: #999;}
    .error ::-moz-placeholder {color: #999}
    .error :-moz-placeholder {color: #999}
    .error :-ms-input-placeholder {color: #999}
  `]
})
export class RegistrationComponent implements OnInit{
  constructor(private userService: UserService, private router: Router) {
  }

  userForm: FormGroup;
  private firstName: FormControl
  private lastName: FormControl
  private email: FormControl
  private password: FormControl
  private passwordAgain: FormControl

  ngOnInit() {
    this.firstName = new FormControl(this.firstName, [Validators.required, Validators.pattern('[a-zA-Z].*')]);
    this.lastName = new FormControl(this.lastName, [Validators.required, Validators.pattern('[a-zA-Z].*')]);
    this.email = new FormControl(this.email, [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]);
    this.password = new FormControl(this.password, Validators.required)
    this.passwordAgain = new FormControl(this.passwordAgain, Validators.required)
    this.userForm = new FormGroup({
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      password: this.password,
      passwordAgain: this.passwordAgain
    })
  }

  validateFirstName() {
    return this.firstName.valid || this.firstName.untouched
  }

  validateLastName() {
    return this.lastName.valid || this.lastName.untouched
  }

  validateEmail() {
    return this.email.valid || this.email.untouched
  }

  validatePassword() {
    return this.password.valid || this.password.untouched
  }

  validatePasswordAgain() {
    return this.passwordAgain.valid || this.passwordAgain.untouched
  }

  checkPassword(userForm) {
    let pass = userForm.password
    let confirmPassword = userForm.passwordAgain
    return pass == confirmPassword;
  }

  isEmpty(userForm) {
    let empty = userForm.email
    if (empty == '' || empty == null) {
      return false
    }
    return true
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

  saveUser(formValues) {
    if (formValues.valid) {
      let objectValue = formValues.value
      delete objectValue['passwordAgain']
      this.userService.saveUser(objectValue).subscribe(() => {
        this.router.navigate(['/'])
      })
      return true
    }
    return false
  }
}
