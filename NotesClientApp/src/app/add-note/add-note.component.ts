import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Note } from '../Models/Note';
import { NotesService } from '../services/notes.service';

@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.css']
})
export class AddNoteComponent implements OnInit {
  public noteTitle: string = 'Новая заметка';
  public noteText: string = '';

  constructor(private notesService: NotesService, private router: Router) { }

  ngOnInit(): void {
  }

  createNoteHandler(): void {
    const note: Note = {
      Id: 0,
      Name: this.noteTitle,
      Text: this.noteText
    }

    this.notesService.create(note).subscribe(() => this.router.navigateByUrl('notes'));
  }

  onCancel(): void {
    this.router.navigateByUrl('notes');
  }
}
