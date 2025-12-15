# Commitments Legacy App

**Angular 21** application for managing personal commitments, built with **Angular Material 21**, tested with **Jest** and **Playwright**.

## 🚀 Tech Stack

- **Angular**: 21.0.0
- **Angular Material**: 21.0.0
- **TypeScript**: 5.9
- **RxJS**: 7.8
- **AG Grid**: 32.0
- **Testing**: Jest 29.7 + Playwright 1.48
- **Build**: ESBuild (via @angular-devkit/build-angular)

## 📋 Prerequisites

- **Node.js**: 18.19+ or 20.11+ or 22.0+
- **npm**: 9.0+ or yarn 3.0+

## 🛠️ Installation

```bash
# Install dependencies
npm install --legacy-peer-deps

# Note: --legacy-peer-deps is currently needed for jest-preset-angular compatibility
```

## 💻 Development

### Development Server

```bash
# Start dev server
npm start

# Navigate to http://localhost:4200/
# The app will automatically reload on file changes
```

### Build

```bash
# Development build
npm run build

# Production build
npm run build:prod
```

### Linting & Formatting

```bash
# Run ESLint
npm run lint

# Fix linting issues
npm run lint:fix

# Format code with Prettier
npm run format

# Check formatting
npm run format:check
```

## 🧪 Testing

### Unit Tests (Jest)

```bash
# Run all unit tests
npm test

# Run tests in watch mode
npm run test:watch

# Generate coverage report
npm run test:coverage
```

See [TESTING.md](./TESTING.md) for detailed testing guidelines.

### E2E Tests (Playwright)

```bash
# Run e2e tests
npm run e2e

# Run in UI mode (interactive)
npm run e2e:ui

# Run in headed mode (see browser)
npm run e2e:headed

# Debug tests
npm run e2e:debug

# Run only Chromium tests
npm run e2e:chromium

# View test report
npm run e2e:report
```

## 📁 Project Structure

```
src/
├── app/
│   ├── achievements/       # Achievements feature module
│   ├── activities/        # Activities feature module
│   ├── commitments/       # Commitments feature module
│   ├── core/             # Core services and utilities
│   ├── shared/           # Shared components and modules
│   └── ...               # Other feature modules
├── assets/               # Static assets
├── environments/         # Environment configurations
├── styles/               # Global styles and themes
│   └── theme.scss       # Material theme configuration
├── main.ts              # Application entry point
└── styles.scss          # Global styles
e2e/
├── pages/               # Page Object Models
├── app.spec.ts         # Basic e2e tests
└── accessibility.spec.ts # Accessibility tests
```

## 🎨 Material Theme

The application uses a custom Angular Material theme defined in `src/styles/theme.scss`.

### Theme Customization

```scss
// Primary color palette
$commitments-primary: mat.m2-define-palette(mat.$m2-indigo-palette);

// Accent color palette
$commitments-accent: mat.m2-define-palette(mat.$m2-pink-palette);
```

### Dark Theme

The application supports dark theme via the `.dark-theme` class.

## 📚 Documentation

- [MIGRATION.md](./MIGRATION.md) - Angular 21 migration guide
- [TESTING.md](./TESTING.md) - Comprehensive testing guide

## 🔄 Recent Updates (Angular 21 Migration)

### Completed ✅
- Upgraded to Angular 21 with ESBuild
- Migrated to Jest for unit testing
- Implemented Playwright for e2e testing
- Updated Material theme to Angular Material 21
- Removed deprecated `entryComponents`
- Migrated from `PortalInjector` to `Injector.create()`
- Added accessibility testing with axe-core

### In Progress 🔧
- Fixing component import paths
- Enabling TypeScript strict mode
- Expanding test coverage
- Performance optimization

See [MIGRATION.md](./MIGRATION.md) for complete migration details.

## 🐛 Known Issues

1. **Legacy Peer Dependencies**: Currently using `--legacy-peer-deps` for jest-preset-angular compatibility
2. **Component Imports**: Some component imports need path updates (in progress)
3. **Strict Mode**: TypeScript strict mode temporarily disabled

## 🤝 Contributing

1. Create a feature branch from `main`
2. Make your changes
3. Run tests: `npm test && npm run e2e`
4. Run linting: `npm run lint`
5. Format code: `npm run format`
6. Create a pull request

## 📄 License

Copyright (c) Quinntyne Brown. All Rights Reserved.
Licensed under the MIT License. See License.txt in the project root for license information.

## 🔗 Resources

### Official Documentation
- [Angular Documentation](https://angular.dev)
- [Angular Material Documentation](https://material.angular.io)
- [Jest Documentation](https://jestjs.io)
- [Playwright Documentation](https://playwright.dev)

### Migration Guides
- [Angular Update Guide](https://update.angular.io)
- [Angular Material Migration](https://material.angular.io/guide/migration)

## 📞 Support

For questions or issues, please check the documentation or create an issue in the repository.
