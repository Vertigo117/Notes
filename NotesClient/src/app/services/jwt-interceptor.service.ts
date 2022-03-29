import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../app.injection-tokens';
import { ACCESS_TOKEN_KEY, AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class JwtInterceptorService implements HttpInterceptor {

  constructor(@Inject(API_URL) private apiUrl: string, private accountService: AccountService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.isValidRequest(req)) {
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${localStorage.getItem(ACCESS_TOKEN_KEY)}`}
      });
    }

    return next.handle(req);
  }

  private isValidRequest(request: HttpRequest<any>): boolean {
    return this.accountService.isAuthenticated;
  }

  private isApiUrl(url: string): boolean {
    return url.startsWith(this.apiUrl);
  }
}
