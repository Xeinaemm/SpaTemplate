import { Component, Input } from '@angular/core';

@Component({
  selector: 'lib-welcome-page',
  template: `
    <div style="text-align:center">
      <h1>{{ title }}!</h1>
      <img width="150" alt="Angular Logo" src="/assets/angular.png" />
    </div>
  `
})
export class WelcomePageComponent {
  @Input()
  title: string;
}
