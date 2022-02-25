import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, tap } from 'rxjs';
import { NOTES_API_URL } from '../app-injection-tokens';
import { ErrorResponse } from '../models/ErrorResponse';
import { TokenDto } from '../models/TokenDto';
import { UserUpsertDto } from '../models/UserUpsertDto';
import { UserLoginDto } from '../models/UserLoginDto';
import { UserDto } from '../models/UserDto';

export const ACCESS_TOKEN_KEY: string = 'access_token';
export const USER_NAME_KEY: string = 'user_name';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    @Inject(NOTES_API_URL) private apiUrl: string,
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService) { }

    public login(credentials: UserLoginDto): Observable<TokenDto> {
      const loginUrl = '/Account/Login';
      return this.httpClient.post<TokenDto>(this.apiUrl + loginUrl, credentials).pipe(tap({
        next: response => {
          localStorage.setItem(ACCESS_TOKEN_KEY, response.Token);
          localStorage.setItem(USER_NAME_KEY, response.UserName);
        },
        error: err => this.handleError(err)
      }));
    }

    private handleError(error: any): void {
      if (error instanceof(HttpErrorResponse)) {
        switch (error.status) {
          case 500:
            const response = error.error as ErrorResponse;
            alert(response.Message);
            break;

          case 400:
            alert(error.error);
            break;
        
          default:
            alert(error.message);
            break;
        }
      } else {
        alert("Something terrible has happened");
      }
    }

    public isAuthenticated(): boolean {
      const token = localStorage.getItem(ACCESS_TOKEN_KEY);
      return token != null && !this.jwtHelper.isTokenExpired(token);
    }

    public logout(): void {
      localStorage.removeItem(ACCESS_TOKEN_KEY);
      localStorage.removeItem(USER_NAME_KEY);
    }

    public register(user: UserUpsertDto): Observable<UserDto> {
      const registerUrl = '/Account/Register';
      return this.httpClient.post<UserDto>(this.apiUrl + registerUrl, user).pipe(tap({
        error: err => this.handleError(err)
      }));
    }
}
