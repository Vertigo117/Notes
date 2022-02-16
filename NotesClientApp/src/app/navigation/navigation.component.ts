import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { LoginDialogComponent } from '../login-dialog/login-dialog.component';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  isAuthenticated: boolean = false;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

    ngOnInit(): void {
      this.isAuthenticated = this.auth.isAuthenticated();
    }

  constructor(
    private breakpointObserver: BreakpointObserver, 
    public dialog: MatDialog, 
    private auth: AuthService,
    private router: Router) {}

  openDialog(): void {
   const dialogRef = this.dialog.open(LoginDialogComponent, {
     height: "50%",
     width: "20%"
   });
   dialogRef.afterClosed().subscribe(() => {
    this.isAuthenticated = this.auth.isAuthenticated();
    if (this.isAuthenticated) {
      this.router.navigate(['notes']);
    }
   });
  }

  logout(): void {
    this.auth.logout();
    this.isAuthenticated = this.auth.isAuthenticated();
    this.router.navigate(['']);
  }

}
