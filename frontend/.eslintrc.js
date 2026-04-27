module.exports = {
  root: true,
  ignorePatterns: [],
  overrides: [
    {
      files: ['*.ts'],
      extends: [
        'eslint:recommended',
        'plugin:@typescript-eslint/recommended',
      ],
      rules: {
        '@typescript-eslint/no-explicit-any': 'warn',
        '@typescript-eslint/no-unused-vars': [
          'error',
          { argsIgnorePattern: '^_' },
        ],
      },
    },
    {
      files: ['*.stories.ts'],
      extends: ['plugin:storybook/recommended'],
      rules: {},
    },
  ],
};
