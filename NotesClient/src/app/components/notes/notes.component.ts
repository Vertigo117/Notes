import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Editor } from 'ngx-editor';
import { NoteDto } from 'src/app/models/NoteDto';
import { NoteUpsertDto } from 'src/app/models/NoteUpsertDto';
import { NotesService } from 'src/app/services/notes.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit, OnDestroy {
  public notes: NoteDto[] = []
  public editor: Editor = new Editor;
  public selectedNoteId: number = 0;

  public noteForm = new FormGroup({
    Name: new FormControl('', [Validators.required]),
    Text: new FormControl(null)
  });

  constructor(private notesService: NotesService) { }
  
  ngOnDestroy(): void {
    this.editor.destroy();
  }

  ngOnInit(): void {
    this.getAllNotes();
  }

  private getAllNotes(): void {
    this.notesService.getAll().subscribe({
      next: response => this.notes = response
    });
  }

  public onNoteClick(note: NoteDto): void {
    let noteUpsert: NoteUpsertDto = {
      Name: note.Name,
      Text: note.Text
    }
    this.noteForm.setValue(noteUpsert);

    this.selectedNoteId = note.Id;
  }

  public onAddClick(): void {
    const note: NoteUpsertDto = {
      Name: 'Новая заметка',
      Text: ''
    }
    this.notesService.create(note).subscribe(() => this.getAllNotes());
  }

  public getSelectedNote(note: NoteDto): boolean {
    return note.Id == this.selectedNoteId;
  }

  public onSaveClick(): void {
    this.notesService.update(this.selectedNoteId, this.noteForm.value).subscribe(() => {
      this.getAllNotes();
      //this.noteForm.reset();
    });
  }

  public onDeleteClick(id: number): void {
    this.notesService.delete(id).subscribe(() => {
      this.getAllNotes();
      this.noteForm.reset();
    });
  }

  public onDateClick(): void {
    this.notes = this.notes.sort((a, b) => this.getTime(a.CreationDate) - this.getTime(b.CreationDate));
  }

  public onNameClick(): void {
    this.notes = this.notes.sort((a, b) => a.Name.localeCompare(b.Name));
  }

  private getTime(date?: Date): number {
    return date != null ? new Date(date).getDate() : 0;
  }
}
