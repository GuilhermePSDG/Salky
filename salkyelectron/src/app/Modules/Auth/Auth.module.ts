import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './Auth-routing.module';
import { LoginOrRegisterComponent } from './Components/LoginOrRegister/login-or-register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [CommonModule, AuthRoutingModule,FormsModule,ReactiveFormsModule],
  declarations: [LoginOrRegisterComponent],
})
export class AuthModule {
  constructor(){
  }
}
