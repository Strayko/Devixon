import {Component, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {UserService} from '../shared/user.service';
import {Router} from '@angular/router';

@Component({
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit{
  constructor(private userService: UserService, private router: Router) {
  }

  passwordRegexFilter
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
    this.password = new FormControl(this.password, [Validators.required, Validators.pattern("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")])
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

  validatePasswordRegx(formValue) {
    let password = formValue['password']
    let uppercase = /(?=.*[A-Z])/.test(password)
    let lowercase = /(?=.*[a-z])/.test(password)
    let digit = /(?=.*\d)/.test(password)
    let limitChar = /(?=.*[a-zA-Z]).{8,}/.test(password)
    return this.userValueComparison(password, uppercase, lowercase, digit, limitChar);
  }

  checkPassword(userForm) {
    let pass = userForm.password
    let confirmPassword = userForm.passwordAgain
    return pass == confirmPassword;
  }

  isEmpty(userForm) {
    let empty = userForm.email
    return (empty == null);
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
      this.userService.save(objectValue).subscribe(() => {
        this.router.navigate(['/'])
      })
      return true
    }
    return false
  }

  private userValueComparison(password, uppercase: boolean, lowercase: boolean, digit: boolean, limitChar: boolean) {
    if (password == null) {
      return this.passwordRegexFilter = '';
    } else if (uppercase && lowercase && digit && limitChar) {
      return this.passwordRegexFilter = '';
    } else if (uppercase && !lowercase && digit && limitChar) {
      return this.passwordRegexFilter = 'lowercase';
    } else if (!uppercase && lowercase && digit && limitChar) {
      return this.passwordRegexFilter = 'uppercase';
    } else if (!uppercase && !lowercase && digit && limitChar) {
      return this.passwordRegexFilter = 'uppercase, lowercase';
    } else if (uppercase && lowercase && !digit && limitChar) {
      return this.passwordRegexFilter = 'digit';
    } else if (!uppercase && !lowercase && !digit && limitChar) {
      return this.passwordRegexFilter = 'uppercase, lowercase, digit';
    } else if (uppercase && !lowercase && !digit && limitChar) {
      return this.passwordRegexFilter = 'lowercase, digit';
    } else if (!uppercase && lowercase && !digit && limitChar) {
      return this.passwordRegexFilter = 'uppercase, digit';
    } else if (!uppercase && lowercase && !digit && !limitChar) {
      return this.passwordRegexFilter = 'uppercase, digit, max 8 char';
    } else if (!uppercase && !lowercase && digit && !limitChar) {
      return this.passwordRegexFilter = 'uppercase, lowercase, max 8 char';
    } else if (!uppercase && lowercase && !digit && limitChar) {
      return this.passwordRegexFilter = 'lowercase, digit, max 8 char';
    } else if (uppercase && lowercase && !digit && !limitChar) {
      return this.passwordRegexFilter = 'digit, max 8 char';
    } else if (uppercase && !lowercase && digit && !limitChar) {
      return this.passwordRegexFilter = 'lowercase, max 8 char';
    } else if (!uppercase && lowercase && !digit && limitChar) {
      return this.passwordRegexFilter = 'uppercase, max 8 char';
    } else if (uppercase && lowercase && digit && !limitChar) {
      return this.passwordRegexFilter = 'max 8 char';
    } else if (!uppercase && !lowercase && !digit && !limitChar) {
      return this.passwordRegexFilter = 'uppercase, lowercase, digit, max 8 char';
    }
  }
}
