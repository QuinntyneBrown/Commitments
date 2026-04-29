# commitments-app Dead-Code Removal — Detailed Design

**Status:** Accepted

## 1. Overview

Goal: delete all dead code left over in `commitments-app` after the feature-library arc (designs 34–42). Every dialog component, dialog service, entity service, entity model, resolver, and shared sub-component that is no longer reachable from any live route or component tree.

After this slice `commitments-app/src/app/` contains only:
- `app.component.*`, `app.routes.ts`, `app.config.ts`
- `components/dashboard-layout/` (alive — used in routes)
- `core/` (auth.service, hub-client, language.service, etc. — still alive)

## 2. Dead items to delete

### components/ (all except dashboard-layout)
- add-tag-dialog, anonymous-master-page, auto-complete-chip-list
- create-profile-dialog, digital-asset-url-input
- edit-activity-dialog, edit-behaviour-dialog, edit-behaviour-type-dialog
- edit-card-dialog, edit-card-layout-dialog, edit-commitment-dialog
- edit-frequency-dialog, edit-to-do-dialog
- frequencies-editor, frequency-editor
- master-page, placeholder-page, quill-text-editor

### services/ (all)
- activity.service, behaviour.service, behaviour-type.service
- card.service, card-layout.service, commitment.service
- create-profile-dialog.service, digital-asset.service
- edit-activity-dialog.service, edit-behaviour-dialog.service
- edit-behaviour-type-dialog.service, edit-card-dialog.service
- edit-card-layout-dialog.service, edit-commitment-dialog.service
- edit-frequency-dialog.service, edit-to-do-dialog.service
- frequency.service, frequency-type.service
- note-resolver.service, notes.service, profile.service
- tags.service, tags-resolver.service, to-do.service

### models/ (all)
All local entity models are now owned by their feature libraries.

### app-store.ts
Only used by the dead master-page component.

## 3. Why "Radically Simple"

Zero new logic. Pure deletion. The test suite stays green because every deleted file was already unreferenced by live production code.
