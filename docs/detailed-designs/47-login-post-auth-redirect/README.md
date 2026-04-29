# Design 47 — Login Post-Auth Redirect + Interceptor Simplification

Status: Complete

## Context

After design 45 (authGuard), the auth guard reads `localStorage.getItem('accessTokenKey')`
directly. But the login page never stores a token after a successful login, and the
headers/JWT interceptors use a complex `LocalStorageService` JSON-array abstraction that
is inconsistent with the guard's raw `localStorage` access.

Result: even a successful login leaves the user stuck on `/login` because the guard always
returns a `UrlTree` redirect.

## Problems

1. `LoginPageComponent.signIn()` calls `auth.token()` but never stores the returned
   `accessToken` and never navigates away.
2. `headers.interceptor.ts` reads via `LocalStorageService.get({name: 'accessTokenKey'})`
   — a JSON-array format that stores everything under `'[Commitments] storageKey'`.
3. `jwt.interceptor.ts` clears via `LocalStorageService.put(... value: null)` — same issue.
4. `LocalStorageService` and `LoginRedirectService` are now over-engineered for what they do.

## Design

### `LoginPageComponent` (identity feature)

```ts
async signIn(): Promise<void> {
  if (this.form.invalid) return;
  try {
    const { accessToken } = await this._auth.token({
      username: this.form.value.username!,
      password: this.form.value.password!,
    });
    localStorage.setItem('accessTokenKey', accessToken);
    this._router.navigate(['/']);
  } catch {
    this.error.set('Login failed');
  }
}
```

### `headers.interceptor.ts` (app core)

```ts
export const headerInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('accessTokenKey') || '';
  return next(req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) }));
};
```

### `jwt.interceptor.ts` (app core)

```ts
export const jwtInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    tap({ error: (e) => {
      if (e instanceof HttpErrorResponse && e.status === 401) {
        localStorage.removeItem('accessTokenKey');
        inject(Router).navigate(['/login']);
      }
    }})
  );
```

### Dead code: delete `local-storage.service.ts` and `redirect.service.ts`

Both are now unused. The core-dead-code-removal spec gains two new assertions.

## Acceptance tests

| File | Test |
|------|------|
| `login-page.component.spec.ts` | successful signIn stores token in localStorage |
| `login-page.component.spec.ts` | successful signIn navigates to `/` |
| `login-page.component.spec.ts` | failed signIn sets error signal, no navigation |
| `headers.interceptor.spec.ts` | sets Authorization header from localStorage |
| `jwt.interceptor.spec.ts` | on 401: clears token, navigates to /login |
| `core-dead-code-removal.spec.ts` | local-storage.service.ts and redirect.service.ts are gone |

## Implementation steps

1. Write failing specs
2. Commit + push
3. Implement changes, delete dead files
4. Verify tests pass → commit + push
