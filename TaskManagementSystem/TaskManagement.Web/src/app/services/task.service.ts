import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Environment } from '../environments/environment';
import { Task, CreateTask, UpdateTask } from '../models/task.model';

@Injectable({
    providedIn: 'root'
})
export class TaskService {

    private readonly apiUrl = `${Environment.apiUrl}`;

    constructor(private http: HttpClient) { }

    getTasks(): Observable<Task[]> {

        return this.http.get<Task[]>(`${Environment.apiUrl}/Tasks/GetAllTasks`);
    }

    createTask(task: CreateTask): Observable<number> {

        return this.http.post<number>(`${Environment.apiUrl}/Tasks/SaveTask`, task);
    }

    updateTask(task: UpdateTask): Observable<void> {

        return this.http.put<void>(`${Environment.apiUrl}/Tasks/UpdateTask`, task);
    }

    deleteTask(id: number): Observable<void> {
        
        return this.http.delete<void>(`${Environment.apiUrl}/Tasks/DeleteTask/${id}`);
    }
}