import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

const meta: Meta = {
  title: 'Material/Card',
  decorators: [
    moduleMetadata({
      imports: [MatCardModule, MatButtonModule, MatIconModule],
    }),
  ],
  parameters: {
    layout: 'padded',
  },
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj;

export const Basic: Story = {
  render: () => ({
    template: `
      <mat-card style="max-width: 400px;">
        <mat-card-header>
          <mat-card-title>Card Title</mat-card-title>
          <mat-card-subtitle>Card Subtitle</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <p>This is a basic card with a title, subtitle, and some content.</p>
        </mat-card-content>
      </mat-card>
    `,
  }),
};

export const WithActions: Story = {
  render: () => ({
    template: `
      <mat-card style="max-width: 400px;">
        <mat-card-header>
          <mat-card-title>Commitment</mat-card-title>
          <mat-card-subtitle>Due: Tomorrow</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <p>Complete the weekly review and plan next week's activities.</p>
        </mat-card-content>
        <mat-card-actions>
          <button mat-button color="primary">COMPLETE</button>
          <button mat-button>RESCHEDULE</button>
        </mat-card-actions>
      </mat-card>
    `,
  }),
};

export const DashboardCard: Story = {
  render: () => ({
    template: `
      <mat-card style="max-width: 300px;">
        <mat-card-header>
          <mat-icon mat-card-avatar style="font-size: 40px; width: 40px; height: 40px;">trending_up</mat-icon>
          <mat-card-title>Daily Results</mat-card-title>
          <mat-card-subtitle>Today's Progress</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <div style="font-size: 48px; font-weight: bold; text-align: center; color: #3f51b5; padding: 20px;">
            85%
          </div>
          <p style="text-align: center; color: #666;">Completion Rate</p>
        </mat-card-content>
        <mat-card-actions align="end">
          <button mat-button color="primary">VIEW DETAILS</button>
        </mat-card-actions>
      </mat-card>
    `,
  }),
};
