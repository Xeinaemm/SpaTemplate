import { Component } from '@angular/core';
import { HelloWorldService } from '../core/hello-world.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'lib-hello-world',
  template: `
    <div style="text-align:center">
      <lib-welcome-page [title]="title"></lib-welcome-page>
      <button mat-raised-button color="primary" (click)="click()">
        Click me!
      </button>
      <mat-progress-spinner
        style="margin:0 auto;"
        [color]="color"
        [mode]="mode"
        [value]="value"
      >
      </mat-progress-spinner>
    </div>
  `
})
export class HelloWorldComponent {
  constructor(private helloWorld: HelloWorldService) {}
  title = 'Welcome page called from library';
  color = 'primary';
  mode = 'determinate';
  value = 0;

  click() {
    this.helloWorld.clickMe().subscribe(x => {
      this.value = x;
    });
  }
}
