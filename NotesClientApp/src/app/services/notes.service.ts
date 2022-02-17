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

  update(id: number, note: Note): Observable<unknown> {
    return this.httpClient.put<Note>(`${this.apiUrl}/Update/${id}`, note);
  }

  delete(id: number): Observable<unknown> {
    return this.httpClient.delete(`${this.apiUrl}/Delete/${id}`);
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
