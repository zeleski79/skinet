import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  loading = false;
  busyRequestCount = 0;

  show() {
    this.busyRequestCount++;
    this.loading = true;
  }

  hide() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.loading = false;
    }
  }
}
