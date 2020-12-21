import {Routes} from '@angular/router';
import {AppComponent} from './dashboard/app.component';
import {RegistrationComponent} from './registration/registration.component';
import {LoginComponent} from './login/login.component';

export const appRoutes: Routes = [
  {path: '', component: AppComponent},
  {path: 'registration', component: RegistrationComponent},
  {path: 'login', component: LoginComponent}
];
