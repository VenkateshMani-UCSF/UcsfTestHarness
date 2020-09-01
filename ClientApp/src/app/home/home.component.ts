import { Component, Inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  authCode = '';

  constructor(readonly router: Router, @Inject(DOCUMENT) readonly document: Document) { }

  public update(value: string) {
    this.authCode = value;
  }

  get window(): Window { return this.document.defaultView; }

  public goToPatientApp() {
    console.log('authCode: ' + this.authCode);
    const url = 'https://localhost:4501/' + '?code=' + this.authCode;
    this.redirect(url, '_blank');
  }

  public redirect(url: string, target = '_blank'): Promise<boolean> {
    return new Promise<boolean>( (resolve, reject) => {
      try {
        resolve(!!this.window.open(url, target));
      } catch (e) {
        reject(e);
      }
    });
  }
}
