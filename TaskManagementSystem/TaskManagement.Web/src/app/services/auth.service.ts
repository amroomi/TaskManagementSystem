import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { Environment } from '../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
  private readonly loginUrl = `${Environment.apiUrl}/auth/validate`;  // ✅ lowercase + correct case

  private authTokenSubject = new BehaviorSubject<string>('');
  public isAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) {
    // Check stored token on startup
    const token = localStorage.getItem('authToken');
    if (token) {
      this.authTokenSubject.next(token);
      this.isAuthenticatedSubject.next(true);
    }
  }

  login(username: string, password: string): Observable<boolean> {
    const requestBody = { username, password };  // ✅ Send credentials in body
    
    // ✅ POST request with JSON body (backend expects this)
    return this.http.post<{ success: boolean; token: string; userId: number; username: string }>(
      this.loginUrl, 
      requestBody
    ).pipe(
      map(response => {
        if (response.success && response.token) {
          localStorage.setItem('authToken', response.token);  // ✅ Store full "Basic ..." token
          this.authTokenSubject.next(response.token);
          this.isAuthenticatedSubject.next(true);
          return true;
        }
        return false;
      }),
      catchError(error => {
        console.error('Login error:', error);
        this.isAuthenticatedSubject.next(false);
        return of(false);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.authTokenSubject.next('');
    this.isAuthenticatedSubject.next(false);
  }

  getAuthHeader(): string {
    return this.authTokenSubject.value;  // ✅ Returns stored "Basic ..." token
  }

  isLoggedIn(): boolean {
    return this.isAuthenticatedSubject.value;
  }
}