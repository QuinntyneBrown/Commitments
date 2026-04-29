import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CommitmentService } from '../../data/commitment.service';
import { Commitment } from '../../data/commitment';

@Component({
  selector: 'commitments-commitments-page',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './commitments-page.component.html',
})
export class CommitmentsPageComponent implements OnInit {
  private readonly _service = inject(CommitmentService);
  readonly commitments = signal<Commitment[]>([]);

  async ngOnInit(): Promise<void> {
    const { commitments } = await this._service.list();
    this.commitments.set(commitments);
  }
}
