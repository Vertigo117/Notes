import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserLoginDto } from 'src/app/Models/UserLoginDto';
import { UserUpsertDto } from 'src/app/Models/UserUpsertDto';
import { AccountService } from 'src/app/services/account.service';
import { NavComponent } from '../nav/nav.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registrationForm: FormGroup = new FormGroup({
    Email: new FormControl('', [Validators.required, Validators.email]),
    Name: new FormControl('', [Validators.required]),
    Password: new FormControl('', [Validators.required])
  });

  public hide = true;

  constructor(public dialogRef: MatDialogRef<NavComponent>, 
    private accountService: AccountService) { }

  ngOnInit(): void {
  }

  public getErrorMessage(): string {
    if (this.registrationForm.hasError('required')) {
      return 'Это поле необходимо заполнить';
    }

    return this.registrationForm.hasError('Email') ? 'Некорректный Email' : '';
  }

  public onCancel(): void {
    this.dialogRef.close();
  }

  public onSubmit(): void {
    this.accountService.register(this.registrationForm.value as UserUpsertDto).subscribe({
      next: response => {
        const credentials: UserLoginDto = { 
          Password: this.registrationForm.controls['Password'].value,
          Email: response.Email
        };
        
        this.accountService.login(credentials).subscribe({
          next: response => {
            if (this.accountService.isAuthenticated()) {
              this.dialogRef.close(response);
            }
          }
        })
      }
    })
  }

}
