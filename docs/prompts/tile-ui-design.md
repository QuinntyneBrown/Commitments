
Ensure the app is fully running, backend and frontend and accessible via url.

Pull any changes from origin

Find a tile in C:\projects\Commitments\docs\tiles. ([current tile])

Add the tile to the running dashboard and ensure it's the only tile

Verify the tile is rendered correctly against the C:\projects\Commitments\docs\tiles\[current tile]\ui-design.pen

    - prioritize major structural issues

        - Is text correctly vertically and horizontally align in buttons, tiles, titles, cards, every where

        - Are the correct components implemented and same as C:\projects\Commitments\docs\tiles\[current tile]\ui-design.pen

        - Are the same buttons, links, menu options the same as C:\projects\Commitments\docs\tiles\[current tile]\ui-design.pen

        - Are all the links triggering the correct behaviour

    - secondary, look for detailed implementation

        - the UI is as close to C:\projects\Commitments\docs\ui-design.pen and design system as much as possible

        - UI components are using Angular material and mostly from C:\projects\Commitments\frontend\projects\commitments-ui

        - padding, margins, fonts, colors, layout etc... is indentical to C:\projects\Commitments\docs\ui-design.pen and design system as much as possible


Log any bugs in C:\projects\Commitments\docs\bugs

For each logged bug in session

    1. Write the failing tests (Unit and / or e2e Playwright Test using Page Object Model) for the fix.

    2. COMMIT ALL CHANGES AND PUSH TO ORIGIN

    3. Implement the simplest radically simple code to fix the bug and make the tests pass

        - TARGETING THE LOWEST COMPLEXITY SCORE POSSIBLE

    4. COMMIT ALL CHANGES AND PUSH TO ORIGIN

    5. Audit the implementation 2 times for radical simplicity

        a) Can the implementation be made more testable or simpler? Fix it

        b) If a UI change, ensure the correct icons visible to user in browser, fonts, spacing, etc...

        c) Can a change reduce the complexity score to make it more approach by a junior dev (1 year experience with Angular)? Change it


    6. Mark bug as fixed

    7. COMMIT ALL CHANGES AND PUSH TO ORIGIN