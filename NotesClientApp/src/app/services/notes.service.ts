import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { NOTES_API_URL } from '../app-injection-tokens';
import { Note } from '../Models/Note';

@Injectable({
  providedIn: 'root'
})
export class NotesService {
  public notes: Note[] = []

  constructor(
    private httpClient: HttpClient, 
    @Inject(NOTES_API_URL) private apiUrl: string,
    private router: Router) { }

  getAll(): Observable<Note[]> {
    return this.httpClient.get<Note[]>(`${this.apiUrl}/GetAll`).pipe(tap({
      next: response => this.notes = response,
      error: err => this.handleError(err)
    }));
  }

  create(note: Note): Observable<Note> {
    return this.httpClient.post<Note>(`${this.apiUrl}/Create`, note);
  }

  handleError(error: any): void {
    if (error instanceof HttpErrorResponse)
    {
      switch (error.status) {
        case 401:
          this.router.navigateByUrl('');
          break;
      
        default:
          break;
      }
    }
  }
}
