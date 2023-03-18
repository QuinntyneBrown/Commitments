// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, Input, Renderer, ElementRef, HostListener } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { takeUntil, tap, map, switchMap } from 'rxjs';
import { MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { ENTER } from '@angular/cdk/keycodes';
import { AuthService } from '../core/auth.service';
import { LoginRedirectService } from '../core/redirect.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../core/error.service';
import { ProfileService } from '../profiles/profile.service';
import { LocalStorageService } from '../core/local-storage.service';
import { currentProfileIdKey } from '../core/constants';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  selector: 'app-login'
})
export class LoginComponent {
  constructor(
    private readonly _authService: AuthService,
    private readonly _elementRef: ElementRef,
    private readonly _errorService: ErrorService,
    private readonly _loginRedirectService: LoginRedirectService,
    private readonly _profileService: ProfileService,
    private readonly _renderer: Renderer,
    private readonly _storage: LocalStorageService
  ) {}

  ngOnInit() {
    this._authService.logout();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngAfterContentInit() {
    this._renderer.invokeElementMethod(this.usernameNativeElement, 'focus', []);
  }

  public username: string;

  public password: string;

  private readonly _snackBarRef: MatSnackBarRef<SimpleSnackBar>;

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
      .handle$(errorResponse, 'Login Failed')
      .pipe(takeUntil(this.onDestroy), map(snackBarRef => (this._snackBarRef = snackBarRef)))
      .subscribe();
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

