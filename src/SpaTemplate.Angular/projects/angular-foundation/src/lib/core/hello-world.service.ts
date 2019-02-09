import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HelloWorldService {
  clickMe(): Observable<number> {
    return new Observable(observer => {
      let value = 0;
      while (value !== 100) {
        value++;
        observer.next(value);
      }
    });
  }
}
