import {Component, ElementRef, ViewChild} from '@angular/core';

@Component({
  selector: 'nav-bar',
  templateUrl: './nav.component.html'
})
export class NavComponent {
  @ViewChild('examplecollapsenavbar') examplecollapsenavbar: ElementRef



  toggleNavbar() {

  }

}
