// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef, HostListener, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarRef, SimpleSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs';
import { AuthService } from '../../../core/auth.service';
import { LoginRedirectService } from '../../../core/redirect.service';
import { ErrorService } from '../../../core/error.service';
import { LocalStorageService } from '../../../core/local-storage.service';
import { currentProfileIdKey } from '../../../core/constants';
import { ProfileService } from '../../../services/profile.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  private readonly _authService = inject(AuthService);
  private readonly _elementRef = inject(ElementRef);
  private readonly _errorService = inject(ErrorService);
  private readonly _loginRedirectService = inject(LoginRedirectService);
  private readonly _profileService = inject(ProfileService);
  private readonly _storage = inject(LocalStorageService);

  ngOnInit() {
    this._authService.logout();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngAfterContentInit() {
    this.usernameNativeElement?.focus();
  }

  public username: string = '';

  public password: string = '';

  private _snackBarRef: MatSnackBarRef<SimpleSnackBar>;

  public form = new FormGroup({
    username: new FormControl(this.username, [Validators.required]),
    password: new FormControl(this.password, [Validators.required])
  });

  public get usernameNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('#username');
  }

  @HostListener('window:click')
  public dismissSnackBar() {
    if (this._snackBarRef) this._snackBarRef.dismiss();
  }

  public tryToLogin($event) {
    this._authService
      .tryToLogin({
        username: $event.value.username,
        password: $event.value.password
      })
      .pipe(
        takeUntil(this.onDestroy),
        switchMap(() => this._profileService.current()),
        tap(profile => this._storage.put({ name: currentProfileIdKey, value: profile.profileId }))
      )
      .subscribe(
        () => this._loginRedirectService.redirectPreLogin(),
        errorResponse => this.handleErrorResponse(errorResponse)
      );
  }

  public handleErrorResponse(errorResponse) {
    this._errorService
      .handle$(errorResponse, 'An error occurred. Try it again.')
      .pipe(takeUntil(this.onDestroy), map(snackBarRef => (this._snackBarRef = snackBarRef)))
      .subscribe();
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
