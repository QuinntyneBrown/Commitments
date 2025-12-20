// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ViewChild } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { CommitmentService } from "./commitment.service";
import { Commitment } from "./commitment";
import { ColDef } from "ag-grid";
import { BehaviourService } from "../behaviours/behaviour.service";
import { Behaviour } from "../behaviours/behaviour";
import { FormGroup, FormControl } from "@angular/forms";
import { FrequencyTypeService } from "../frequencies/frequency-type.service";
import { FrequencyType } from "../frequencies/frequency-type";
import { Frequency } from "../frequencies/frequency";
import { CommitmentFrequency } from "./commitment-frequency";
import { FrequencyService } from "../frequencies/frequency.service";
import { map, takeUntil } from "rxjs";

@Component({
  templateUrl: "./edit-commitment-dialog.html",
  styleUrls: ["./edit-commitment-dialog.scss"],
  selector: "app-edit-commitment-dialog"
})
export class EditCommitmentDialog {
  constructor(
    private readonly _overlay: OverlayRefWrapper,
    private readonly _behaviourService: BehaviourService,
    private readonly _commitmentService: CommitmentService,
    private readonly _frequencyService: FrequencyService
  ) { }

  ngOnInit() {
    this.behaviours$ = this._behaviourService.get();
    this.frequencies$ = this._frequencyService.get();
  }

  public commitmentId: number;

  private readonly _commitment = new Commitment();

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public onNgModelChange($event) {
    console.log($event);
  }

  public handleSaveClick(behaviours) {
    this._commitment.commitmentFrequencies = this.frequencies.selectedOptions.selected.map(x => new CommitmentFrequency(x.value.frequencyId, 0));
    this._commitment.behaviourId = this.behaviours.selectedOptions.selected.map(x => x.value.behaviourId)[0];

    this._commitmentService.save({ commitment: this._commitment })
      .pipe(map(x => {
        this._commitment.commitmentId = x.commitmentId;
        this._overlay.close(this._commitment);
      }), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleFrequenciesEditorChange(frequencies: Array<Frequency>) {
    //this._commitment.frequencies = frequencies.map(x => new CommitmentFrequency());
  }


  @ViewChild("behaviours")
  public behaviours: any;

  @ViewChild("frequencies")
  public frequencies: any;

  public behaviours$: Observable<Array<Behaviour>>;
  public frequencies$: Observable<Array<Frequency>>;
  public commitments$: Observable<Array<Commitment>>;

  public form: FormGroup = new FormGroup({
    behaviourId: new FormControl(null, []),
  });
}

