import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { LoginRequest, LoginResponse, User } from '../models/auth.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
  private tokenKey = 'auth_token';
  private userSubject = new BehaviorSubject<User | null>(null);
  
  public user$ = this.userSubject.asObservable();
  
  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.loadUserFromStorage();
  }
  
  login(credentials: LoginRequest): Observable<boolean> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          localStorage.setItem(this.tokenKey, response.token);
          this.loadUserFromToken(response.token);
        }),
        map(() => true),
        catchError(() => of(false))
      );
  }
  
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.userSubject.next(null);
    this.router.navigate(['/login']);
  }
  
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }
  
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  
  private loadUserFromStorage(): void {
    const token = this.getToken();
    if (token) {
      this.loadUserFromToken(token);
    }
  }
  
  private loadUserFromToken(token: string): void {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      const user: User = {
        id: parseInt(payload.nameid, 10),
        username: payload.sub,
        isAdmin: payload.role === 'Admin'
      };
      
      this.userSubject.next(user);
    } catch (error) {
      console.error('Error parsing token', error);
      this.logout();
    }
  }
}