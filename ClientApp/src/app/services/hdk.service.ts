import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class HDKService {

    constructor(private httpClient: HttpClient) { }

    getAuthorizationCode(launchCode: string): Observable<any> {
        let hdkUrl = `api/hdk/getAuth?launchCode=${launchCode ? launchCode : ''}`;
        return this.httpClient.get<any>(hdkUrl);
    }
}