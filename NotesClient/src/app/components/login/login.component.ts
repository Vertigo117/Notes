import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserLoginDto } from 'src/app/Models/UserLoginDto';
import { AccountService } from 'src/app/services/account.service';
import { NavComponent } from '../nav/nav.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup = new FormGroup({
    Email: new FormControl('', [Validators.required, Validators.email]),
    Password: new FormControl('', [Validators.required])
  });

  public hide = true;

  constructor(
    public dialogRef: MatDialogRef<NavComponent>, 
    private accountService: AccountService) { }

  ngOnInit(): void {
  }

  public getErrorMessage(): string {
    if (this.loginForm.hasError('required')) {
      return 'Это поле необходимо заполнить';
    }

    return this.loginForm.hasError('Email') ? 'Некорректный Email' : '';
  }

  public onSubmit(): void {
    const user = this.loginForm.value as UserLoginDto;
    this.accountService.login(user).subscribe({
      next: (response) => {
        if (this.accountService.isAuthenticated()) {
          this.dialogRef.close(response);
        }
      }
    });
  }

  public onCancel(): void {
    this.dialogRef.close();
  }

}
