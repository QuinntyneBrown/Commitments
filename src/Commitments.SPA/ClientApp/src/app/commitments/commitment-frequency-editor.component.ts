import { Component, Input, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import { FrequencyType } from "./frequency-type.model";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { CommitmentFrequency } from "./commitment-frequency.model";

@Component({
  templateUrl: "./commitment-frequency-editor.component.html",
  styleUrls: ["./commitment-frequency-editor.component.css"],
  selector: "app-commitment-frequency-editor"
})
export class CommitmentFrequencyEditorComponent { 

  public onDestroy: Subject<void> = new Subject<void>();
  
  @Input()
  public frequencyTypes: Array<FrequencyType> = [];

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public handleSaveClick() {
    var commitmentFrequency = new CommitmentFrequency();
    commitmentFrequency.frequency = this.form.value.frequency;
    commitmentFrequency.frequencyTypeId = this.form.value.frequencyTypeId;
    this.save.emit({ commitmentFrequency });
  }

  public save: EventEmitter<any> = new EventEmitter();

  public form: FormGroup = new FormGroup({
    frequency: new FormControl(null, [Validators.required]),
    frequencyTypeId: new FormControl(null,[Validators.required])
  });
}
