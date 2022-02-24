import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NOTES_API_URL } from '../app-injection-tokens';
import { NoteDto } from '../Models/NoteDto';
import { NoteUpsertDto } from '../Models/NoteUpsertDto';

@Injectable({
  providedIn: 'root'
})
export class NotesService {

  constructor(
    @Inject(NOTES_API_URL) private apiUrl: string,
    private httpClient: HttpClient) 
  { }

  public getAll(): Observable<NoteDto[]> {
    const getAllUrl = '/api/Notes/GetAll';
    return this.httpClient.get<NoteDto[]>(this.apiUrl + getAllUrl);
  }

  public create(note: NoteUpsertDto): Observable<NoteDto> {
    const createUrl = '/api/Notes/Create';
    return this.httpClient.post<NoteDto>(this.apiUrl + createUrl, note);
  }
  
  public update(id: number, note: NoteUpsertDto): Observable<unknown> {
    const updateUrl = `/api/Notes/Update/${id}`;
    return this.httpClient.put(this.apiUrl + updateUrl, note);
  }

  public delete(id: number): Observable<unknown> {
    const deleteUrl = `/api/Notes/Delete/${id}`;
    return this.httpClient.delete(this.apiUrl + deleteUrl);
  }
}
