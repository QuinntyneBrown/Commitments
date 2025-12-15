# Testing Guide

## Overview
The Commitments application uses Jest for unit testing and Playwright for end-to-end testing.

## Unit Testing with Jest

### Running Tests
```bash
# Run all tests
npm test

# Run tests in watch mode
npm run test:watch

# Run tests with coverage
npm run test:coverage
```

### Writing Component Tests

#### Basic Component Test
```typescript
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { MyComponent } from './my.component';

describe('MyComponent', () => {
  let component: MyComponent;
  let fixture: ComponentFixture<MyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MyComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MyComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render title', () => {
    component.title = 'Test Title';
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Test Title');
  });
});
```

#### Testing with Dependencies
```typescript
import { TestBed } from '@angular/core/testing';
import { MyService } from './my.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('MyService', () => {
  let service: MyService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MyService],
    });

    service = TestBed.inject(MyService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch data', () => {
    const mockData = [{ id: 1, name: 'Test' }];

    service.getData().subscribe(data => {
      expect(data).toEqual(mockData);
    });

    const req = httpMock.expectOne('api/data');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });
});
```

### Testing Material Components

#### Using Material Harnesses
```typescript
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';
import { MatButtonHarness } from '@angular/material/button/testing';
import { MatDialogHarness } from '@angular/material/dialog/testing';

it('should open dialog when button clicked', async () => {
  const loader = TestbedHarnessEnvironment.loader(fixture);
  const button = await loader.getHarness(MatButtonHarness.with({ text: 'Open Dialog' }));
  
  await button.click();
  
  const dialog = await loader.getHarness(MatDialogHarness);
  expect(await dialog.getRole()).toBe('dialog');
});
```

### Mocking with Jest
```typescript
// Mock a service
const mockService = {
  getData: jest.fn().mockReturnValue(of([])),
  saveData: jest.fn().mockReturnValue(of({})),
} as jest.Mocked<MyService>;

// Use in tests
TestBed.configureTestingModule({
  providers: [
    { provide: MyService, useValue: mockService }
  ]
});

// Verify calls
expect(mockService.getData).toHaveBeenCalled();
expect(mockService.saveData).toHaveBeenCalledWith({ id: 1 });
```

### Best Practices
1. **Test behavior, not implementation**: Focus on what the component does, not how
2. **Use descriptive test names**: Use "should..." format
3. **Keep tests isolated**: Each test should be independent
4. **Mock external dependencies**: Don't make real HTTP calls or access localStorage
5. **Test edge cases**: Include null, undefined, empty arrays, etc.
6. **Aim for 80%+ coverage**: But don't sacrifice quality for coverage

## E2E Testing with Playwright

### Running Tests
```bash
# Run all e2e tests
npm run e2e

# Run in UI mode (interactive)
npm run e2e:ui

# Run in headed mode (see browser)
npm run e2e:headed

# Run in debug mode
npm run e2e:debug

# Run only Chromium tests
npm run e2e:chromium

# View test report
npm run e2e:report
```

### Writing E2E Tests

#### Page Object Model Pattern
```typescript
// pages/my-page.page.ts
import { Page, Locator } from '@playwright/test';

export class MyPage {
  readonly page: Page;
  readonly submitButton: Locator;
  readonly nameInput: Locator;

  constructor(page: Page) {
    this.page = page;
    this.submitButton = page.getByRole('button', { name: /submit/i });
    this.nameInput = page.getByLabel('Name');
  }

  async goto() {
    await this.page.goto('/my-page');
    await this.page.waitForLoadState('networkidle');
  }

  async fillForm(name: string) {
    await this.nameInput.fill(name);
  }

  async submit() {
    await this.submitButton.click();
  }
}
```

#### Using Page Objects in Tests
```typescript
// my-feature.spec.ts
import { test, expect } from '@playwright/test';
import { MyPage } from './pages/my-page.page';

test.describe('My Feature', () => {
  let myPage: MyPage;

  test.beforeEach(async ({ page }) => {
    myPage = new MyPage(page);
    await myPage.goto();
  });

  test('should submit form successfully', async ({ page }) => {
    await myPage.fillForm('John Doe');
    await myPage.submit();

    await expect(page.getByText('Success!')).toBeVisible();
  });
});
```

### Accessibility Testing

#### Basic Accessibility Test
```typescript
import { test, expect } from '@playwright/test';
import AxeBuilder from '@axe-core/playwright';

test('should not have accessibility violations', async ({ page }) => {
  await page.goto('/');

  const accessibilityScanResults = await new AxeBuilder({ page })
    .withTags(['wcag2a', 'wcag2aa'])
    .analyze();

  expect(accessibilityScanResults.violations).toEqual([]);
});
```

#### Testing Keyboard Navigation
```typescript
test('should navigate with keyboard', async ({ page }) => {
  await page.goto('/');
  
  // Tab through interactive elements
  await page.keyboard.press('Tab');
  await expect(page.getByRole('button').first()).toBeFocused();
  
  // Press Enter
  await page.keyboard.press('Enter');
  
  // Verify action occurred
  await expect(page.getByRole('dialog')).toBeVisible();
});
```

### Visual Regression Testing
```typescript
test('should match screenshot', async ({ page }) => {
  await page.goto('/');
  
  // Take screenshot and compare
  await expect(page).toHaveScreenshot('home-page.png');
});

test('should match element screenshot', async ({ page }) => {
  await page.goto('/');
  
  const header = page.locator('header');
  await expect(header).toHaveScreenshot('header.png');
});
```

### API Mocking
```typescript
test('should handle API responses', async ({ page }) => {
  // Mock API response
  await page.route('**/api/data', route => {
    route.fulfill({
      status: 200,
      body: JSON.stringify([{ id: 1, name: 'Test' }]),
    });
  });

  await page.goto('/');
  
  // Verify data is displayed
  await expect(page.getByText('Test')).toBeVisible();
});
```

### Cross-Browser Testing
Playwright automatically runs tests across multiple browsers as configured in `playwright.config.ts`:
- Chromium (Desktop Chrome)
- Firefox
- WebKit (Safari)
- Mobile Chrome (Pixel 5)
- Mobile Safari (iPhone 12)

### Best Practices
1. **Use Page Object Model**: Encapsulate selectors and actions
2. **Use role-based selectors**: More resilient than CSS selectors
3. **Wait for network idle**: Use `waitForLoadState('networkidle')`
4. **Test user journeys**: Focus on complete workflows
5. **Keep tests independent**: Each test should set up its own state
6. **Use descriptive names**: Clearly describe what is being tested
7. **Handle flakiness**: Use retries for flaky tests in CI

## Coverage Requirements

### Unit Tests
- **Minimum Coverage**: 80%
- **Focus Areas**:
  - All services: 90%+
  - Business logic: 95%+
  - Components: 75%+
  - Utilities: 100%

### E2E Tests
- **Critical User Journeys**: 100%
- **Major Features**: 80%+
- **Edge Cases**: As needed

## Continuous Integration

Tests run automatically on:
- Every push to any branch
- Every pull request
- Before merging to main

### CI Configuration
See `.github/workflows/ci.yml` for the complete CI pipeline.

## Debugging Tests

### Debugging Jest Tests
```bash
# Run a specific test file
npm test my-component.spec.ts

# Run tests matching a pattern
npm test -- --testNamePattern="should create"

# Run with verbose output
npm test -- --verbose
```

### Debugging Playwright Tests
```bash
# Run in debug mode
npm run e2e:debug

# Run specific test
npx playwright test my-test.spec.ts --debug

# Generate trace
npx playwright test --trace on
npx playwright show-trace trace.zip
```

## Test Organization

```
src/
├── app/
│   ├── components/
│   │   ├── my-component/
│   │   │   ├── my-component.component.ts
│   │   │   └── my-component.component.spec.ts
│   │   └── ...
│   ├── services/
│   │   ├── my-service/
│   │   │   ├── my-service.service.ts
│   │   │   └── my-service.service.spec.ts
│   │   └── ...
│   └── ...
e2e/
├── pages/
│   ├── home.page.ts
│   └── ...
├── app.spec.ts
├── accessibility.spec.ts
└── ...
```

## Resources

- [Jest Documentation](https://jestjs.io/docs/getting-started)
- [jest-preset-angular](https://github.com/thymikee/jest-preset-angular)
- [Playwright Documentation](https://playwright.dev/docs/intro)
- [Angular Testing Guide](https://angular.dev/guide/testing)
- [axe-core Playwright](https://github.com/dequelabs/axe-core-npm/tree/develop/packages/playwright)

## Getting Help

If you encounter issues with tests:
1. Check the test output for specific errors
2. Review the relevant documentation
3. Check existing tests for examples
4. Ask the team in the #testing channel
