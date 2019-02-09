import { NgModule, Optional, SkipSelf } from '@angular/core';
import { SharedModule } from './shared.module';

@NgModule({
	declarations: [],
	imports: [SharedModule]
})
export class CoreModule {
	constructor(
		@Optional()
		@SkipSelf()
		parentModule: CoreModule
	) {
		if (parentModule) {
			throw new Error('CoreModule is already loaded. Import only in AppModule');
		}
	}
}
