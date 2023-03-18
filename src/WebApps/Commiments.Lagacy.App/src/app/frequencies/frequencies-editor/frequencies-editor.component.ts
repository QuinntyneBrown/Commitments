// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, Input } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FrequencyType } from "./frequency-type";
import { Frequency } from "./frequency";
import { ColDef } from "ag-grid";

@Component({
  templateUrl: "./frequencies-editor.component.html",
  styleUrls: ["./frequencies-editor.component.scss"],
  selector: "app-frequencies-editor"
})
export class FrequenciesEditorComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  @Input()
  public frequencyTypes: Array<FrequencyType> = [];

  @Input()
  public frequencies$: BehaviorSubject<Array<Frequency>> = new BehaviorSubject([]);

  @Input()
  public frequencies: any[] = [];

  public handleFrequencySave($event) {
    this.frequencies$.next([...this.frequencies$.value,$event.frequency]);
  }

  public onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public handleSaveClick() {
    
  }

  public remove($event) {
    const frequencies: any[] = [...this.frequencies$.value];
    const index = frequencies.findIndex(x => x.frequency == $event.data.frequency && x.frequencyTypeId == $event.data.frequencyTypeId);    
    frequencies.splice(index, 1);
    this.frequencies$.next(frequencies);
  }

  public columnDefs: Array<ColDef> = [
    {
      headerName: 'Frequency',
      field: 'frequency'
    },
    {
      headerName: 'Frequency Type',
      field: 'frequencyTypeId'
    },
    {      
      template: `<a>Remove</a>`,
      onCellClicked: frequency => this.remove(frequency)
    }
  ];
}

