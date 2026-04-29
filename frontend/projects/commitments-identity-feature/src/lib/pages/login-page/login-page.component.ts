import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../data/auth.service';

@Component({
  selector: 'commitments-login-page',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './login-page.component.html',
})
export class LoginPageComponent {
  private readonly _auth = inject(AuthService);

  readonly form = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  readonly error = signal<string | null>(null);

  async signIn(): Promise<void> {
    if (this.form.invalid) return;
    try {
      await this._auth.token({
        username: this.form.value.username!,
        password: this.form.value.password!,
      });
    } catch {
      this.error.set('Login failed');
    }
  }
}
