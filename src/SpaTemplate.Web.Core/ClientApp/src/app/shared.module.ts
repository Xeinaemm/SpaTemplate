import { NgModule } from '@angular/core';
import { FoundationModule } from 'angular-foundation';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
	imports: [FoundationModule, NgbModule],
	exports: [FoundationModule, NgbModule]
})
export class SharedModule {}
