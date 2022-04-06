import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-Picture',
  templateUrl: './Picture.component.html',
  styleUrls: ['./Picture.component.scss']
})
export class PictureComponent implements OnInit {

  constructor() { }

  @Input() source = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
  @Input() altText = "Foto"
  @Input() Width = 150;
  @Input() Height = 150;
  ngOnInit() {
  }

}
