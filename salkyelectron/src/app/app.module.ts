import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChatBoxComponent } from './Components/chat-box/chat-box.component';
import { SendMessageBoxComponent } from './Components/send-message-box/send-message-box.component';
import { RoundedPictureComponent } from './Components/Shared/Picture/rounded-picture.component';
import { ContactsListComponent } from './Components/contacts-list/contacts-list.component';
import { ScrenCaptureComponent } from './Components/scren-capture/scren-capture.component';
import { AudioService } from './Services/AudioService';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UserComponent } from './Components/user/user.component';
import { LoginComponent } from './Components/user/login/login.component';
import { RegisterComponent } from './Components/user/register/register.component';
import { ProfileComponent } from './Components/user/profile/profile.component';
import { LogoutComponent } from './Components/user/logout/logout.component';
import { SalkyWebSocketChat } from './Services/SalkyWebSocketChat';
import { UserService } from './Services/UserService.service';
import { HttpInterceptorInterceptor } from './Interceptors/http-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ChatBoxComponent,
    SendMessageBoxComponent,
    ContactsListComponent,
    RoundedPictureComponent,
    ScrenCaptureComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
    LogoutComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
  ],
  providers: [
    AudioService,
    SalkyWebSocketChat,
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  
}
