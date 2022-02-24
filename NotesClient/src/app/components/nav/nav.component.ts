import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { RegisterComponent } from '../register/register.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {

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

    
    public userName = 'Неавторизован';

  public onLogin(): void {
    const dialogRef = this.dialog.open(LoginComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (this.accountService.isAuthenticated()) {
        this.userName = result.UserName;
      }
    });
  }

  public onLogout(): void {
    this.accountService.logout();
    this.userName = 'Неавторизован';
    this.router.navigateByUrl('/home');
  }

  public onRegister(): void {
    const dialogRef = this.dialog.open(RegisterComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (this.accountService.isAuthenticated()) {
        this.userName = result.UserName;
      }
    })
  }
}
