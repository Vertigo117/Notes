import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { NavigationComponent } from '../navigation/navigation.component';
import { FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { RegisterDialogComponent } from '../register-dialog/register-dialog.component';

export interface IAuthData {
}

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
export class LoginDialogComponent implements OnInit {

  emailControl = new FormControl('', [Validators.required, Validators.email]);
  passwordControl = new FormControl('', Validators.required);
  hide = true;
  isInLoadingState = false;

  constructor(
    public dialogRef: MatDialogRef<NavigationComponent>,
    public dialog: MatDialog,
    public auth: AuthService) {}

  ngOnInit(): void {
  }

  getErrorMessage(): string {
    if (this.emailControl.hasError('required') || this.passwordControl.hasError('required')) {
      return 'Это поле необходимо заполнить';
    }

    return this.emailControl.hasError('email') ? 'Адрес электронной почты указан неверно' : '';
  }

  onLogin(email: string, password: string): void {
    this.isInLoadingState = true;
    this.auth.login(email, password).subscribe({
      next: () => {
        this.dialogRef.close();
        this.isInLoadingState = false;
      },
      error: () => this.isInLoadingState = false
    });
  }

  onRegister(): void {
    this.dialogRef.close();
    this.dialog.open(RegisterDialogComponent);
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
