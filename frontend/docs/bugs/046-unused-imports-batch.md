---
id: 046
title: Eight unused imports across the codebase trip @typescript-eslint/no-unused-vars
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 359ead4
flow: dashboard-layout
severity: low
---

# Eight unused imports across the codebase trip @typescript-eslint/no-unused-vars

## Summary

After bug 044 + 045 cleared the project-meaningful lint errors,
eight `'XYZ' is defined but never used` errors remain — each a
straightforward unused import:

| File | Line | Symbol |
|---|---|---|
| `checkbox-cell.component.ts` | 8 | `IAfterGuiAttachedParams` |
| `dashboard-card-configuration-dialog.component.ts` | 10 | `Validators` |
| `digital-asset-url-input.component.ts` | 4 | `AfterViewInit` |
| `master-page.component.ts` | 4 | `Inject` |
| `auth.service.ts` | 4 | `Inject` |
| `tags-page.component.ts` | 13 | `filter` |
| `note-resolver.service.ts` | 6 | `Observable` |
| `tags-resolver.service.ts` | 6 | `Observable` |

None of these symbols is referenced in their respective file
bodies — they're vestigial from refactors. The lint rule is
correctly flagging them.

## Reproduction

```
cd frontend
npm run lint 2>&1 | awk '/^C:/{file=$0} /is defined but never used/{ if(!/Allowed unused/) print file" :: "$0 }'
```

…returns the eight matches.

## Fix outline (radically simple)

Surgical Edit per file: drop each unused name from its
`import { … }` clause. If the clause becomes empty after the
removal, drop the entire `import` line.

## Tests to add (failing first)

`npm run lint` itself is the artefact: pre-fix it reports the
eight errors; post-fix they're gone. The full e2e + jest
suites must remain green to prove no behavioural regression.
