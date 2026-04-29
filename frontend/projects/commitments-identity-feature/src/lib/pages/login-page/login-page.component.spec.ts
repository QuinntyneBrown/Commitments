// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injector, runInInjectionContext } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../data/auth.service';
import { LoginPageComponent } from './login-page.component';

function makeComponent(authToken: jest.Mock, navigate: jest.Mock) {
  const injector = Injector.create({
    providers: [
      { provide: AuthService, useValue: { token: authToken } },
      { provide: Router, useValue: { navigate } },
    ]
  });
  return runInInjectionContext(injector, () => new LoginPageComponent());
}

describe('LoginPageComponent', () => {
  beforeEach(() => localStorage.clear());
  afterEach(() => localStorage.clear());

  it('stores the access token in localStorage on success', async () => {
    const navigate = jest.fn();
    const authToken = jest.fn().mockResolvedValue({ accessToken: 'tok-123', profileId: 'p1' });
    const comp = makeComponent(authToken, navigate);

    comp.form.setValue({ username: 'alice', password: 'pw' });
    await comp.signIn();

    expect(localStorage.getItem('accessTokenKey')).toBe('tok-123');
  });

  it('navigates to / on success', async () => {
    const navigate = jest.fn();
    const authToken = jest.fn().mockResolvedValue({ accessToken: 'tok-123', profileId: 'p1' });
    const comp = makeComponent(authToken, navigate);

    comp.form.setValue({ username: 'alice', password: 'pw' });
    await comp.signIn();

    expect(navigate).toHaveBeenCalledWith(['/']);
  });

  it('sets error signal and does not navigate on failure', async () => {
    const navigate = jest.fn();
    const authToken = jest.fn().mockRejectedValue(new Error('401'));
    const comp = makeComponent(authToken, navigate);

    comp.form.setValue({ username: 'bad', password: 'bad' });
    await comp.signIn();

    expect(comp.error()).toBe('Login failed');
    expect(navigate).not.toHaveBeenCalled();
  });
});
