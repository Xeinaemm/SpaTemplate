import { NgModule } from '@angular/core';
import { SharedFoundationModule } from './shared.module';
import { HelloWorldComponent } from './features/hello-world.component';

@NgModule({
  declarations: [HelloWorldComponent],
  imports: [SharedFoundationModule],
  exports: [HelloWorldComponent]
})
export class FeaturesFoundationModule {}
