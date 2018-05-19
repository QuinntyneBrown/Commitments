import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { FrequencyTypeService } from "./frequency-type.service";
import { FrequencyType } from "./frequency-type.model";

@Component({
  templateUrl: "./frequency-page.component.html",
  styleUrls: ["./frequency-page.component.css"],
  selector: "app-frequency-page"
})
export class FrequencyPageComponent { 
  constructor(private _frequencyTypeService: FrequencyTypeService) { }

  ngOnInit() {
    this.frequencyTypes$ = this._frequencyTypeService.get();
  }

  public frequencyTypes$: Observable<Array<FrequencyType>>;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
