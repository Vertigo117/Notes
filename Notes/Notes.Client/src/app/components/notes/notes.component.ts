import { Component, OnDestroy, OnInit } from '@angular/core';
import { NoteDto } from 'src/app/models/note-dto.model';
import { NotesService } from 'src/app/services/notes.service';
import { Editor } from 'ngx-editor';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NoteUpsertDto } from 'src/app/models/note-upsert-dto.model';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent implements OnInit, OnDestroy {

  public notes!: NoteDto[];
  public selectedNote?: NoteDto;
  public editor: Editor = new Editor;

  public editorForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    text: new FormControl(null)
  });

  constructor(private notesService: NotesService, private snackBar: MatSnackBar) { }
  
  ngOnDestroy(): void {
    this.editor.destroy();
  }

  ngOnInit(): void {
    this.getNotes();
  }

  private getNotes(): void {
    this.notesService.getAll().subscribe((response) => this.notes = response);
  }

  public onNoteClick(note: NoteDto): void {
    this.selectedNote = note;
    let noteUpsertDto: NoteUpsertDto = {
      name: note.name,
      text: note.text
    }
    this.editorForm.setValue(noteUpsertDto);
  }

  public onSaveClick(): void {
    if (this.selectedNote) {
      this.notesService
        .update(this.selectedNote.id, this.editorForm.value as NoteUpsertDto)
        .subscribe(() => {
          this.getNotes();
          this.openSnackBar('Изменения сохранены.');
        });
    }
  }

  private openSnackBar(message: string): void {
    let config: MatSnackBarConfig<any> = {
      duration: 3000
    }
    this.snackBar.open(message, 'Ok', config);
  }

  public onAddClick(): void {
    const note: NoteUpsertDto = {
      name: 'Новая заметка',
      text: ''
    }
    this.notesService.create(note).subscribe(() => this.getNotes());
  }

  public onDeleteClick(id: number): void {
    this.notesService.delete(id).subscribe(() => {
      this.getNotes();
      this.editorForm.reset();
      this.openSnackBar('Заметка удалена.')
    });
  }

}
