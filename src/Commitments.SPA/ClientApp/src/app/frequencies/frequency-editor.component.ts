import { Component, Input, EventEmitter, Output } from "@angular/core";
import { Subject } from "rxjs";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Frequency } from "./frequency.model";
import { FrequencyType } from "./frequency-type.model";

@Component({
  templateUrl: "./frequency-editor.component.html",
  styleUrls: ["./frequency-editor.component.css"],
  selector: "app-frequency-editor"
})
export class FrequencyEditorComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  public frequency: number;

  @Input()
  public frequencyTypes: Array<FrequencyType> = [];

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  public handleSaveClick() {
    let frequency = new Frequency();
    frequency.frequency = this.form.value.frequency;
    frequency.frequencyTypeId = this.form.value.frequencyTypeId;
    frequency.isDesired = this.form.value.isDesired;
    this.save.emit({ frequency });
  }


  @Output() 
  public save: EventEmitter<any> = new EventEmitter();

  public form: FormGroup = new FormGroup({
    frequency: new FormControl(null, [Validators.required]),
    frequencyTypeId: new FormControl(null, [Validators.required]),
    isDesired: new FormControl(true, [Validators.required])
  });
}
