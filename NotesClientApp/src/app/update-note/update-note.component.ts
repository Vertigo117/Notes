import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Note } from '../Models/Note';
import { NotesService } from '../services/notes.service';

@Component({
  selector: 'app-update-note',
  templateUrl: './update-note.component.html',
  styleUrls: ['./update-note.component.css']
})
export class UpdateNoteComponent implements OnInit {
  public note!: Note;

  constructor(private notesService: NotesService, private router: Router) { }

  ngOnInit(): void {
    this.note = history.state;
  }

  updateNoteHandler(): void {
    this.notesService.update(this.note.Id, this.note).subscribe(() => this.router.navigateByUrl('notes'));
  }

}
