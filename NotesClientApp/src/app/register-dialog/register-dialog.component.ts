import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { User } from '../Models/User';
import { NavigationComponent } from '../navigation/navigation.component';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register-dialog',
  templateUrl: './register-dialog.component.html',
  styleUrls: ['./register-dialog.component.css']
})
export class RegisterDialogComponent implements OnInit {
  emailControl = new FormControl('', [Validators.required, Validators.email]);
  passwordControl = new FormControl('', Validators.required);
  hide = true;
  isInLoadingState = false;
  userName: string = '';

  constructor(
    private authService: AuthService,
    public dialogRef: MatDialogRef<NavigationComponent>,
    private router: Router) { }

  ngOnInit(): void {
  }

  getErrorMessage(): string {
    if (this.emailControl.hasError('required') || this.passwordControl.hasError('required')) {
      return 'Это поле необходимо заполнить';
    }

    return this.emailControl.hasError('email') ? 'Адрес электронной почты указан неверно' : '';
  }

  onRegister(email: string, password: string): void {
    const user: User = {
      Email: email,
      Password: password,
      Name: this.userName
    }
    this.authService.register(user).subscribe({
      next: () => {
        this.authService.login(user.Email, user.Password).subscribe({
          next: () => {
            if (this.authService.isAuthenticated())
            {
              this.router.navigateByUrl('notes');
              this.dialogRef.close();
            }
          }
        })
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

}
