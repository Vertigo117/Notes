import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../app.injection-tokens';
import { NoteDto } from '../models/note-dto.model';
import { NoteUpsertDto } from '../models/note-upsert-dto.model';
import { PagedNotesDto } from '../models/paged-notes-dto.model';

@Injectable({
  providedIn: 'root'
})
export class NotesService {

  constructor(@Inject(API_URL) private apiUrl: string, private httpClient: HttpClient) { }

  public getPaged(skip: number, take: number): Observable<PagedNotesDto> {
    return this.httpClient.get<PagedNotesDto>(`${this.apiUrl}/Notes/Get`, {
      params: {skip, take}
    });
  }

  public getById(id: number): Observable<NoteDto> {
    return this.httpClient.get<NoteDto>(`${this.apiUrl}/Notes/Get/${id}`);
  }

  public create(noteUpsertDto: NoteUpsertDto): Observable<NoteDto> {
    return this.httpClient.post<NoteDto>(`${this.apiUrl}/Notes/Create`, noteUpsertDto);
  }

  public update(id: number, noteUpsertDto: NoteUpsertDto): Observable<unknown> {
    return this.httpClient.put(`${this.apiUrl}/Notes/Update/${id}`, noteUpsertDto);
  }

  public delete(id: number): Observable<unknown> {
    return this.httpClient.delete(`${this.apiUrl}/Notes/Delete/${id}`);
  }
}
