import { Component, Inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  authCode = '';
  launchCode = '';

  constructor(readonly router: Router, @Inject(DOCUMENT) readonly document: Document) { }

  public updateAuth(value: string) {
    this.authCode = value;
  }

  public updateLaunch(value: string) {
    this.launchCode = value;
  }

  get window(): Window { return this.document.defaultView; }

  public goToPatientAppWithAuthCode() {
    console.log('authCode: ' + this.authCode);
    const url = 'https://localhost:4501/' + '?code=' + this.authCode + '&state=PatientApp state';
    this.redirect(url, '_blank');
  }

  public goToPatientAppWithLaunchCode() {
    console.log('authCode: ' + this.authCode);
    const url = 'https://localhost:4501/' + '?launch=' + this.launchCode;
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
