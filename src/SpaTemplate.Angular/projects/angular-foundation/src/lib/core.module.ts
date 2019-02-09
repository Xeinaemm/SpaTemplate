import { NgModule, Optional, SkipSelf } from '@angular/core';
import { SharedFoundationModule } from './shared.module';
import { HelloWorldService } from './core/hello-world.service';

@NgModule({
  providers: [HelloWorldService],
  imports: [SharedFoundationModule]
})
export class CoreFoundationModule {
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreFoundationModule
  ) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import only in AppModule');
    }
  }
}
