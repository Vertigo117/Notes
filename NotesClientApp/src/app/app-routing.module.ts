import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddNoteComponent } from './add-note/add-note.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { UpdateNoteComponent } from './update-note/update-note.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'notes' , component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'add-note', component: AddNoteComponent, canActivate: [AuthGuard] },
  { path: 'update-note', component: UpdateNoteComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
