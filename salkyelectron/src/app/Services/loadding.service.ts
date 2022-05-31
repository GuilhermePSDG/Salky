import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoaddingService {
  public overlay: any;
  public loadingBox: any;
  public windowLoadded = false;

  constructor() {
    window.addEventListener('load', () => {
      this.overlay = document.getElementById('overlay-main-loadding');
      this.loadingBox = document.getElementById('loader-main');
      this.windowLoadded = true;
    });
  }

  public Show() {
    if (this.windowLoadded) {
      this.overlay.style.display = 'flex';
      this.loadingBox.style.display = 'flex';
    }
  }

  public Hide() {
    if (this.windowLoadded) {
      this.overlay.style.display = 'none';
      this.loadingBox.style.display = 'none';
    }
  }
}
