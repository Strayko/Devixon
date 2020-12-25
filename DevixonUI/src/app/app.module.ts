import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './dashboard/app.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { DevixonAppComponent } from './devixon-app.component';
import { RegistrationComponent } from './registration/registration.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserService } from './shared/user.service';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './login/login.component';
import { NavComponent } from './nav/nav.component';
import { DetailsComponent } from './user-details/details.component';
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import {ErrorInterceptor} from './_helpers/error.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LoginComponent,
    DevixonAppComponent,
    NavComponent,
    DetailsComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes),
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    UserService,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ],
  bootstrap: [DevixonAppComponent]
})
export class AppModule { }
