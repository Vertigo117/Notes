import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_URL } from '../app.injection-tokens';
import { Result } from '../models/result.model';
import { TokenDto } from '../models/token-dto.model';
import { UserDto } from '../models/user-dto.model';
import { UserLoginDto } from '../models/user-login-dto.model';
import { UserUpsertDto } from '../models/user-upsert-dto.model';

export const ACCESS_TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    @Inject(API_URL) private apiUrl: string, private httpClient: HttpClient) { }

  public login(userLoginDto: UserLoginDto): Observable<Result<TokenDto>> {
    return this.httpClient.post<Result<TokenDto>>(`${this.apiUrl}/Account/Login`, userLoginDto).pipe(tap({
      next: response => localStorage.setItem(ACCESS_TOKEN_KEY, response.data.token)
    }))
  }

  public logout(): void {
    if (this.isAuthenticated) {
      localStorage.removeItem(ACCESS_TOKEN_KEY);
    }
  }

  public register(userUpsertDto: UserUpsertDto): Observable<Result<UserDto>> {
    return this.httpClient.post<Result<UserDto>>(`${this.apiUrl}/Account/Register`, userUpsertDto);
  }

  public get isAuthenticated(): boolean {
    return localStorage.getItem(ACCESS_TOKEN_KEY) !== null;
  }
}
