import { NgModule } from '@angular/core';
import { CoreFoundationModule } from './core.module';
import { SharedFoundationModule } from './shared.module';
import { FeaturesFoundationModule } from './features.module';

@NgModule({
  exports: [
    CoreFoundationModule,
    SharedFoundationModule,
    FeaturesFoundationModule
  ]
})
export class FoundationModule {}
