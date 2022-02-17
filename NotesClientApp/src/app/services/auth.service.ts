import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AUTH_API_URL } from '../app-injection-tokens';
import { LoginResponse } from '../Models/LoginResponse';
import { User } from '../Models/User';

export const ACCESS_TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private httpClient: HttpClient,
    @Inject(AUTH_API_URL) private apiUrl: string,
    private JwtHelper: JwtHelperService,
    private router: Router) 
  { }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/Login`, {
      email, password
    }).pipe(tap({
      next: response => localStorage.setItem(ACCESS_TOKEN_KEY, response.Token),
      error: err => this.handleError(err)
    }));
  }

  register(user: User): Observable<User> {
    return this.httpClient.post<User>(`${this.apiUrl}/Register`, user).pipe(tap({
      error: err => this.handleError(err)
    }))
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return token != null && !this.JwtHelper.isTokenExpired(token);
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
  }

  private handleError(error: any) {
    if (error instanceof HttpErrorResponse) {
      if (error.error instanceof ErrorEvent) {
          console.error("Error Event");
      } else {
          console.log(`error status : ${error.status} ${error.statusText}`);
          switch (error.status) {
              case 401:      //login
                  this.router.navigateByUrl('');
                  break;
              case 403:     //forbidden
                  this.router.navigateByUrl('');
                  break;
              case 400:
                  alert(error.error);
                  return;
          }
      } 
  } else {
      console.error("some thing else happened");
  }
  return throwError(() => new Error(error.error));
  }
}
