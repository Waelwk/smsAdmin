import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { addIcons } from 'ionicons';
import {
  alertCircleOutline,
  arrowBackOutline,
  carOutline,
  cardOutline,
  checkmarkOutline,
  chevronDownOutline,
  chevronForwardOutline,
  createOutline,
  downloadOutline,
  flashOutline,
  helpCircleOutline,
  homeOutline,
  informationCircleOutline,
  keyOutline,
  notificationsOutline,
  peopleOutline,
  searchOutline,
  settingsOutline,
  warningOutline
} from 'ionicons/icons';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';

addIcons({
  'alert-circle-outline': alertCircleOutline,
  'arrow-back-outline': arrowBackOutline,
  'car-outline': carOutline,
  'card-outline': cardOutline,
  'checkmark-outline': checkmarkOutline,
  'chevron-down-outline': chevronDownOutline,
  'chevron-forward-outline': chevronForwardOutline,
  'create-outline': createOutline,
  'download-outline': downloadOutline,
  'flash-outline': flashOutline,
  'help-circle-outline': helpCircleOutline,
  'home-outline': homeOutline,
  'information-circle-outline': informationCircleOutline,
  'key-outline': keyOutline,
  'notifications-outline': notificationsOutline,
  'people-outline': peopleOutline,
  'search-outline': searchOutline,
  'settings-outline': settingsOutline,
  'warning-outline': warningOutline
});

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    CommonModule,
    HttpClientModule,
    RouterModule,
    IonicModule.forRoot({ mode: 'md' }),
    AppRoutingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule {}
