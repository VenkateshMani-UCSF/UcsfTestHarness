import { Component } from '@angular/core';
import { HDKService } from '../services/hdk.service';

@Component({
    selector: 'app-patient',
    templateUrl: './PatientApp.component.html',
})
export class PatientAppComponent {

    constructor(private hdkService: HDKService) { }

    launchCode: string;

    authResponse = {
        code: '',
        state: ''
    };

    launchPatientApp() {
        this.hdkService.getAuthorizationCode(this.launchCode)
            .subscribe((res) => {
                alert(JSON.stringify(res));
                this.authResponse = res;
                window.location.href = `https://localhost:4501/#/?code=${this.authResponse.code}&state=${this.authResponse.state}`;
            }, error => {
                alert(`Error occred. Details => ${JSON.stringify(error)}`);
            });
    }
}