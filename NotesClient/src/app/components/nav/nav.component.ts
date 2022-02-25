import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { AccountService, USER_NAME_KEY } from 'src/app/services/account.service';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { RegisterComponent } from '../register/register.component';
import { Router } from '@angular/router';

export const DEFAULT_USER_NAME = 'Не авторизован';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private breakpointObserver: BreakpointObserver, 
    public accountService: AccountService,
    private dialog: MatDialog,
    private router: Router) {}
  
    ngOnInit(): void {
    if (this.accountService.isAuthenticated()) {
      this.userName = this.getUserName();
    }
  }
    
    public userName = DEFAULT_USER_NAME;

  public onLogin(): void {
    const dialogRef = this.dialog.open(LoginComponent);
    dialogRef.afterClosed().subscribe(() => {
      if (this.accountService.isAuthenticated()) {
        this.userName = this.getUserName();
      }
    });
  }

  public onLogout(): void {
    this.accountService.logout();
    this.userName = DEFAULT_USER_NAME;
    this.router.navigateByUrl('/home');
  }

  public onRegister(): void {
    const dialogRef = this.dialog.open(RegisterComponent);
    dialogRef.afterClosed().subscribe(() => {
      if (this.accountService.isAuthenticated()) {
        this.userName = this.getUserName();
      }
    })
  }

  private getUserName(): string {
    return localStorage.getItem(USER_NAME_KEY) ?? DEFAULT_USER_NAME;
  }
}
