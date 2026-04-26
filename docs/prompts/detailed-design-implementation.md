


Pull any changes from origin

Find a detailed design in C:\projects\gers\docs\detailed-designs starting at C:\projects\Commitments\docs\detailed-designs\01-live-goal-metrics

that is not implemented respecting the sequential order.

Mark the design as accepted.

Commit all changes and push.


Implement the detailed design using Acceptance Test Driven Development.

    1. Write the failing tests (Unit and / or e2e Playwright Test using Page Object Model).

    2. commit and push to origin

    3. Implement the simplest radically simple code to make the tests pass

        - TARGETING THE LOWEST COMPLEXITY SCORE POSSIBLE

    4. commit and push to origin

    5. Audit the implementation 5 times for radical simplicity and completeness against the detail design, docs and C:\projects\gers\docs\ui-design.pen.

        a) Can the implementation be made more testable or simpler? Fix it

        b) If a UI change, ensure the correct icons visible to user in browser, fonts, spacing, etc...

        c) Can a change reduce the complexity score to make it more approach by a junior dev (1 year experience with Angular)? Change it


    6. Mark detailed design as complete

    7. commit and push to origin