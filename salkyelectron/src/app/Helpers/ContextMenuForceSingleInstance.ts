import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ForceSingle {
  CloseF?: () => void;

  public InformOpen(Close: () => void) {
    if (this.CloseF) this.CloseF();
    this.CloseF = Close;
  }
  public InformClose() {
    this.CloseF = undefined;
  }
}
