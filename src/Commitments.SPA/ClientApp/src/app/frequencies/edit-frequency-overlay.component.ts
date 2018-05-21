import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { FrequencyType } from "./frequency-type.model";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { FrequencyTypeService } from "./frequency-type.service";
import { FrequencyService } from "./frequency.service";
import { takeUntil, map } from "rxjs/operators";

@Component({
  templateUrl: "./edit-frequency-overlay.component.html",
  styleUrls: ["./edit-frequency-overlay.component.css"],
  selector: "app-edit-frequency-overlay"
})
export class EditFrequencyOverlayComponent { 
  constructor(public _overlay: OverlayRefWrapper,
    public frequencyTypeService: FrequencyTypeService,
    public frequencyService: FrequencyService
  ) { }

  ngOnInit() {
    this.frequencyTypes$ = this.frequencyTypeService.get();
  }

  public frequencyTypes$: Observable<Array<FrequencyType>>;

  public handleSave($event) {    
    this.frequencyService.save({ frequency: $event.frequency })
      .pipe(map(x => {
        $event.frequency.frequencyId = x.frequencyId;
        this._overlay.close($event.frequency);
      }))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}
