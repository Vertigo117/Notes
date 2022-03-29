import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { ErrorDialogComponent } from '../components/error-dialog/error-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingInterceptorService implements HttpInterceptor {

  constructor(private dialog: MatDialog, private router: Router) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(tap({
      error: err => this.handleError(err)
    }));
  }

  private handleError(error: any): void {
    if (error instanceof(HttpErrorResponse)) {
      switch (error.status) {
        case 500:
          let response = error.error as Error;
          this.dialog.open(ErrorDialogComponent, { data: response });
          break;

        case 400:
          this.dialog.open(ErrorDialogComponent, { data: { message: error.error, stackTrace: ''} })
          break;

        case 401:
          const dialogRef = this.dialog.open(ErrorDialogComponent, { 
            data: { 
              message: 'Вы не авторизованы в системе!' 
            }});
          dialogRef.afterClosed().subscribe(() => this.router.navigateByUrl('/login'))
          break;
      
        default:
          this.dialog.open(ErrorDialogComponent, { data: { message: error.message, stackTrace: '' }})
          break;
      }
    } else {
      alert("Something terrible has happened");
    }
  }
}
