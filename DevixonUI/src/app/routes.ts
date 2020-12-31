import {Routes} from '@angular/router';
import {AppComponent} from './dashboard/app.component';
import {RegistrationComponent} from './registration/registration.component';
import {LoginComponent} from './login/login.component';
import {DetailsComponent} from './user-details/details.component';
import {AuthGuard} from './_guards/auth.guard';
import {LoggedGuard} from './_guards/logged.guard';

export const appRoutes: Routes = [
  {path: '', component: AppComponent},
  {path: 'registration', component: RegistrationComponent, canActivate: [LoggedGuard]},
  {path: 'login', component: LoginComponent, canActivate: [LoggedGuard]},
  {path: 'user/details', component: DetailsComponent, canActivate: [AuthGuard]}
];
