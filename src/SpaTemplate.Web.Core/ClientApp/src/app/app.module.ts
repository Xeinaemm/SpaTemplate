import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app.routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core.module';
import { FeaturesModule } from './features.module';
import { SharedModule } from './shared.module';

@NgModule({
	declarations: [AppComponent],
	imports: [BrowserModule, AppRoutingModule, CoreModule, FeaturesModule, SharedModule],
	bootstrap: [AppComponent]
})
export class AppModule {}
