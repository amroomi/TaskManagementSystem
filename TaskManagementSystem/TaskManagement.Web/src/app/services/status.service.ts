import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Environment } from '../environments/environment';
import { TaskStatus } from '../models/status.model';

@Injectable({
    providedIn: 'root'
})
export class StatusService {

    private readonly apiUrl = `${Environment.apiUrl}/TaskStatuses/GetAllStatuses`;

    constructor(private http: HttpClient) { }

    getStatuses(): Observable<TaskStatus[]> {
        
        return this.http.get<TaskStatus[]>(this.apiUrl);
    }
}