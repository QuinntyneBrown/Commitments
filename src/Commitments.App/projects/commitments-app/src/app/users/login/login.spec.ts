// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Login } from './login';

describe('Login', () => {
  let mockAuthService: any;
  let mockElementRef: any;
  let mockErrorService: any;
  let mockLoginRedirectService: any;
  let mockProfileService: any;
  let mockRenderer: any;
  let mockStorage: any;

  beforeEach(() => {
    mockAuthService = { tryToLogin: jest.fn(), logout: jest.fn() };
    mockElementRef = { nativeElement: { querySelector: jest.fn() } };
    mockErrorService = { handle$: jest.fn() };
    mockLoginRedirectService = { redirectPreLogin: jest.fn() };
    mockProfileService = { current: jest.fn() };
    mockRenderer = { invokeElementMethod: jest.fn() };
    mockStorage = { get: jest.fn(), put: jest.fn() };
  });

  it('should be defined', () => {
    expect(Login).toBeDefined();
  });

  it('should have form with username and password controls', () => {
    const component = new Login(
      mockAuthService,
      mockElementRef,
      mockErrorService,
      mockLoginRedirectService,
      mockProfileService,
      mockRenderer,
      mockStorage
    );

    expect(component.form).toBeDefined();
    expect(component.form.contains('username')).toBeTruthy();
    expect(component.form.contains('password')).toBeTruthy();
  });

  it('should require username', () => {
    const component = new Login(
      mockAuthService,
      mockElementRef,
      mockErrorService,
      mockLoginRedirectService,
      mockProfileService,
      mockRenderer,
      mockStorage
    );

    const usernameControl = component.form.get('username');
    usernameControl?.setValue('');
    expect(usernameControl?.valid).toBeFalsy();
  });

  it('should require password', () => {
    const component = new Login(
      mockAuthService,
      mockElementRef,
      mockErrorService,
      mockLoginRedirectService,
      mockProfileService,
      mockRenderer,
      mockStorage
    );

    const passwordControl = component.form.get('password');
    passwordControl?.setValue('');
    expect(passwordControl?.valid).toBeFalsy();
  });

  it('should call logout on init', () => {
    const component = new Login(
      mockAuthService,
      mockElementRef,
      mockErrorService,
      mockLoginRedirectService,
      mockProfileService,
      mockRenderer,
      mockStorage
    );

    component.ngOnInit();
    expect(mockAuthService.logout).toHaveBeenCalled();
  });
});
