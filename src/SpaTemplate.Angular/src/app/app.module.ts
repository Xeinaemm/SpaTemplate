import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { CoreModule } from './core.module';
import { FeaturesModule } from './features.module';
import { SharedModule } from './shared.module';

@NgModule({
  declarations: [AppComponent],
  imports: [CoreModule, FeaturesModule, SharedModule],
  bootstrap: [AppComponent]
})
export class AppModule {}
