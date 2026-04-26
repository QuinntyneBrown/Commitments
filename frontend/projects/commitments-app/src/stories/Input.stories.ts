import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const meta: Meta = {
  title: 'Material/Input',
  decorators: [
    moduleMetadata({
      imports: [MatInputModule, MatFormFieldModule, MatIconModule, FormsModule, ReactiveFormsModule],
    }),
  ],
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj;

export const Basic: Story = {
  render: () => ({
    template: `
      <mat-form-field appearance="outline" style="width: 300px;">
        <mat-label>Enter text</mat-label>
        <input matInput placeholder="Type here...">
      </mat-form-field>
    `,
  }),
};

export const WithHint: Story = {
  render: () => ({
    template: `
      <mat-form-field appearance="outline" style="width: 300px;">
        <mat-label>Email</mat-label>
        <input matInput placeholder="user@example.com">
        <mat-hint>We'll never share your email</mat-hint>
      </mat-form-field>
    `,
  }),
};

export const WithIcon: Story = {
  render: () => ({
    template: `
      <mat-form-field appearance="outline" style="width: 300px;">
        <mat-label>Search</mat-label>
        <mat-icon matPrefix>search</mat-icon>
        <input matInput placeholder="Search...">
      </mat-form-field>
    `,
  }),
};

export const Textarea: Story = {
  render: () => ({
    template: `
      <mat-form-field appearance="outline" style="width: 400px;">
        <mat-label>Description</mat-label>
        <textarea matInput placeholder="Enter a description..." rows="4"></textarea>
        <mat-hint align="end">0 / 500</mat-hint>
      </mat-form-field>
    `,
  }),
};
