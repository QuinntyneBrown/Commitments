// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

interface SidenavItem {
  label: string;
  icon: string;
  routerLink: string;
  exact?: boolean;
}

@Component({
  selector: 'app-dashboard-layout',
  standalone: true,
  imports: [NgClass, RouterLink, RouterLinkActive, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-layout.component.html',
  styleUrls: ['./dashboard-layout.component.scss']
})
export class DashboardLayoutComponent {
  protected readonly sidenavOpen = signal(true);

  protected readonly navItems: ReadonlyArray<SidenavItem> = [
    { label: 'Dashboard', icon: 'dashboard', routerLink: '/', exact: true },
    { label: 'Activities', icon: 'directions_run', routerLink: '/activities' },
    { label: 'Behaviours', icon: 'psychology', routerLink: '/behaviours' },
    { label: 'Behaviour Types', icon: 'category', routerLink: '/behaviour-types' },
    { label: 'Commitments', icon: 'task_alt', routerLink: '/commitments' },
    { label: 'Cards', icon: 'style', routerLink: '/cards' },
    { label: 'Card Layouts', icon: 'dashboard_customize', routerLink: '/card-layouts' },
    { label: 'Frequencies', icon: 'schedule', routerLink: '/frequencies' },
    { label: 'Notes', icon: 'sticky_note_2', routerLink: '/notes' },
    { label: 'Profiles', icon: 'group', routerLink: '/profiles' },
    { label: "To Do's", icon: 'checklist', routerLink: '/to-dos' },
    { label: 'Logout', icon: 'logout', routerLink: '/login' }
  ];

  protected toggleSidenav(): void {
    this.sidenavOpen.update(v => !v);
  }
}
