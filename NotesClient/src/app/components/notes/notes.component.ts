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

  public notes: NoteDto[] = [];
  public selectedNote?: NoteDto;
  public editor: Editor = new Editor;
  public isInLoadingState = false;
  public total: number = 0;
  public skip: number = 0;
  public take: number = 10;

  public editorForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    text: new FormControl(null)
  });

  constructor(private notesService: NotesService, private snackBar: MatSnackBar) { }
  
  ngOnDestroy(): void {
    this.editor.destroy();
  }

  ngOnInit(): void {
    this.getNotes(this.skip, this.take);
  }

  private getNotes(skip: number, take: number): void {
    this.isInLoadingState = true;
    this.notesService.getPaged(skip, take).subscribe({
      next: response => {
        this.notes = this.notes.concat(response.notes);
        this.total = response.total;
        this.skip += response.notes.length;
        this.isInLoadingState = false;
      },
      error: () => this.isInLoadingState = false
    });
  }

  public onShowMoreClick(): void {
    this.getNotes(this.skip, this.take);
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
    if (!this.editorForm.valid) {
      return;
    }

    if (this.selectedNote) {
      let noteUpsertDto = this.editorForm.value as NoteUpsertDto;
      this.notesService
        .update(this.selectedNote.id, noteUpsertDto).subscribe(() => {
          this.updateNotes(noteUpsertDto);
          this.openSnackBar('Изменения сохранены.');
        });
    }
  }

  private updateNotes(noteUpsertDto: NoteUpsertDto): void {
    let note = this.notes.find(note => note.id == this.selectedNote?.id);
    if (note) {
      note.name = noteUpsertDto.name;
      note.text = noteUpsertDto.text;
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
    this.notesService.create(note).subscribe((response) => this.notes.push(response));
  }

  public onDeleteClick(id: number): void {
    this.notesService.delete(id).subscribe(() => {
      this.notes.splice(this.notes.findIndex(note => note.id == id));
      this.selectedNote = undefined;
      this.openSnackBar('Заметка удалена.')
    });
  }

}
