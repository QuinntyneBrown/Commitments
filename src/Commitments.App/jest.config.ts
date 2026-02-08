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
    '!projects/commitments-app/src/**/*.spec.ts',
    '!projects/commitments-app/src/main.ts',
    '!projects/commitments-app/src/polyfills.ts',
    '!projects/commitments-app/src/environments/**',
    '!projects/commitments-app/src/test.ts',
  ],
  coverageDirectory: 'coverage',
  coverageReporters: ['html', 'text', 'lcov', 'json'],
  coveragePathIgnorePatterns: ['/node_modules/', '/dist/', '/coverage/'],
  testMatch: ['**/projects/commitments-app/src/**/*.spec.ts'],
  testPathIgnorePatterns: ['/node_modules/', '/e2e/'],
  moduleNameMapper: {
    '^@app/(.*)$': '<rootDir>/projects/commitments-app/src/app/$1',
    '^@environments/(.*)$': '<rootDir>/projects/commitments-app/src/environments/$1',
    '^ag-grid$': '<rootDir>/__mocks__/ag-grid.ts',
  },
  transformIgnorePatterns: ['node_modules/(?!.*\\.mjs$)'],
  passWithNoTests: true,
};

export default config;
