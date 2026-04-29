import type { Config } from 'jest';

const config: Config = {
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
  testEnvironment: 'jsdom',
  transform: {
    '^.+\\.(ts|js|html)$': [
      'jest-preset-angular',
      {
        tsconfig: '<rootDir>/projects/commitments-app/tsconfig.spec.json',
        stringifyContentPathRegex: '\\.(html|svg)$',
      },
    ],
  },
  moduleFileExtensions: ['ts', 'html', 'js', 'json', 'mjs'],
  collectCoverageFrom: [
    'projects/commitments-app/src/**/*.ts',
    'projects/commitments-ui/src/**/*.ts',
    'projects/dashboard-framework/src/**/*.ts',
    '!projects/commitments-app/src/**/*.spec.ts',
    '!projects/commitments-ui/src/**/*.spec.ts',
    '!projects/dashboard-framework/src/**/*.spec.ts',
    '!projects/commitments-app/src/main.ts',
    '!projects/commitments-app/src/polyfills.ts',
    '!projects/commitments-app/src/environments/**',
  ],
  coverageDirectory: 'coverage',
  coverageReporters: ['html', 'text', 'lcov', 'json'],
  coveragePathIgnorePatterns: ['/node_modules/', '/dist/', '/coverage/'],
  testMatch: [
    '**/projects/commitments-app/src/**/*.spec.ts',
    '**/projects/commitments-ui/src/**/*.spec.ts',
    '**/projects/dashboard-framework/src/**/*.spec.ts',
    '**/projects/commitments-dashboard-plugin/src/**/*.spec.ts',
  ],
  testPathIgnorePatterns: ['/node_modules/', '/e2e/'],
  modulePathIgnorePatterns: ['<rootDir>/dist/'],
  moduleNameMapper: {
    '^@app/(.*)$': '<rootDir>/projects/commitments-app/src/app/$1',
    '^@environments/(.*)$': '<rootDir>/projects/commitments-app/src/environments/$1',
    '^@commitments/ui$': '<rootDir>/projects/commitments-ui/src/public-api.ts',
    '^@commitments/dashboard-framework$':
      '<rootDir>/projects/dashboard-framework/src/public-api.ts',
    '^@commitments/dashboard-plugin$':
      '<rootDir>/projects/commitments-dashboard-plugin/src/public-api.ts',
  },
  transformIgnorePatterns: ['node_modules/(?!.*\\.mjs$)'],
  passWithNoTests: true,
};

export default config;
