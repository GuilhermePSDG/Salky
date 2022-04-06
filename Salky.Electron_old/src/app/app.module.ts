import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InputBaseComponent } from './Components/Shared/InputBase/InputBase.component';
import { RoundedPictureComponent } from './Components/Shared/RoundedPicture/RoundedPicture.component';

@NgModule({
  declarations: [
    AppComponent,
    InputBaseComponent,
    RoundedPictureComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
