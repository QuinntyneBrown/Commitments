# 09 — Login page audit (out of scope for the reported issues, but listed for completeness)

**Design**: `Login — LG` (`8xz6c`), centered `loginCard` on `$bg`. Responsive variants exist at S/M/LG/XL (`YfR0i`, `oC1bM`, `8xz6c`, `rKU7X`).

**Implementation**: `LoginPageComponent` is referenced by routes but was not audited in this pass.

## Tasks
- [x] **9.1** Audit `pages/login/login-page/login-page.component.*` against `Login — S/M/LG/XL` frames. Produce a follow-up checklist in this folder (e.g. `09a-login-detailed-audit.md`) covering: card width / radius / shadow, input style, button style, copy, responsive behaviour at each breakpoint. → [09a-login-detailed-audit.md](09a-login-detailed-audit.md)

See [_reference.md](_reference.md) for tokens and node IDs.
