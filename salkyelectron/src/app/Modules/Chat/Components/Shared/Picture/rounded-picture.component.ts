import { Component, Input, OnInit, ɵallowSanitizationBypassAndThrow, ɵbypassSanitizationTrustResourceUrl, ɵgetSanitizationBypassType } from '@angular/core';

@Component({
  selector: 'app-rounded-picture',
  templateUrl: './rounded-picture.component.html',
  styleUrls: ['./rounded-picture.component.scss']
})
export class RoundedPictureComponent implements OnInit {

  constructor() { }

  @Input() source? : string;
  @Input() altText = "Foto"
  @Input() Size = 45;
  ngOnInit() {
    }

}
