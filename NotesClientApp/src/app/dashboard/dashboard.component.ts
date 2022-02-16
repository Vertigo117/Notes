import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Note } from '../Models/Note';
import { NotesService } from '../services/notes.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public isInLoadingState: boolean = true;

  constructor(
    public notesService: NotesService,
    private router: Router) {}

  ngOnInit(): void {
      this.notesService.getAll().subscribe(() => this.isInLoadingState = false);
  }

  addNoteHandler(): void {
    this.router.navigateByUrl('add-note');
  }
}
