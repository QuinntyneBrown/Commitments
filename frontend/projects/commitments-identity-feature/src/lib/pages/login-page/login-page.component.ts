import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../data/auth.service';

@Component({
  selector: 'commitments-login-page',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSnackBarModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
})
export class LoginPageComponent {
  private readonly _auth = inject(AuthService);
  private readonly _router = inject(Router);
  private readonly _snackBar = inject(MatSnackBar);

  readonly form = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  async signIn(): Promise<void> {
    if (this.form.invalid) return;
    try {
      const { accessToken, profileId } = await this._auth.token({
        username: this.form.value.username!,
        password: this.form.value.password!,
      });
      localStorage.setItem('accessTokenKey', accessToken);
      if (profileId) localStorage.setItem('profileId', profileId);
      this._router.navigate(['/']);
    } catch {
      this._snackBar.open('Login Failed', 'Dismiss', { panelClass: ['snackbar--error'] });
    }
  }
}
