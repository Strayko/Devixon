import {Routes} from '@angular/router';
import {AppComponent} from './dashboard/app.component';
import {RegistrationComponent} from './registration/registration.component';

export const appRoutes: Routes = [
  {path: '', component: AppComponent},
  {path: 'registration', component: RegistrationComponent}
];
