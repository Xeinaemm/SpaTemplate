import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { WelcomePageComponent } from './shared/welcome-page.component';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [WelcomePageComponent],
  imports: [
    CommonModule,
    BrowserModule,
    MatProgressSpinnerModule,
    MatButtonModule
  ],
  exports: [
    CommonModule,
    BrowserModule,
    MatProgressSpinnerModule,
    WelcomePageComponent,
    MatButtonModule
  ]
})
export class SharedFoundationModule {}
