import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

const meta: Meta = {
  title: 'Material/Button',
  decorators: [
    moduleMetadata({
      imports: [MatButtonModule, MatIconModule],
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
      <div style="display: flex; gap: 16px; flex-wrap: wrap;">
        <button mat-button>Basic</button>
        <button mat-button color="primary">Primary</button>
        <button mat-button color="accent">Accent</button>
        <button mat-button color="warn">Warn</button>
        <button mat-button disabled>Disabled</button>
      </div>
    `,
  }),
};

export const Raised: Story = {
  render: () => ({
    template: `
      <div style="display: flex; gap: 16px; flex-wrap: wrap;">
        <button mat-raised-button>Basic</button>
        <button mat-raised-button color="primary">Primary</button>
        <button mat-raised-button color="accent">Accent</button>
        <button mat-raised-button color="warn">Warn</button>
        <button mat-raised-button disabled>Disabled</button>
      </div>
    `,
  }),
};

export const Flat: Story = {
  render: () => ({
    template: `
      <div style="display: flex; gap: 16px; flex-wrap: wrap;">
        <button mat-flat-button>Basic</button>
        <button mat-flat-button color="primary">Primary</button>
        <button mat-flat-button color="accent">Accent</button>
        <button mat-flat-button color="warn">Warn</button>
        <button mat-flat-button disabled>Disabled</button>
      </div>
    `,
  }),
};

export const IconButtons: Story = {
  render: () => ({
    template: `
      <div style="display: flex; gap: 16px; flex-wrap: wrap; align-items: center;">
        <button mat-icon-button><mat-icon>home</mat-icon></button>
        <button mat-icon-button color="primary"><mat-icon>favorite</mat-icon></button>
        <button mat-icon-button color="accent"><mat-icon>star</mat-icon></button>
        <button mat-icon-button color="warn"><mat-icon>delete</mat-icon></button>
      </div>
    `,
  }),
};

export const FAB: Story = {
  render: () => ({
    template: `
      <div style="display: flex; gap: 16px; flex-wrap: wrap; align-items: center;">
        <button mat-fab><mat-icon>add</mat-icon></button>
        <button mat-fab color="primary"><mat-icon>edit</mat-icon></button>
        <button mat-fab color="accent"><mat-icon>favorite</mat-icon></button>
        <button mat-fab color="warn"><mat-icon>delete</mat-icon></button>
      </div>
    `,
  }),
};
