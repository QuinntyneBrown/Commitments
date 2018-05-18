import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { CommitmentService } from "./commitment.service";
import { Commitment } from "./commitment.model";
import { takeUntil, tap } from "rxjs/operators";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";

@Component({
  templateUrl: "./edit-commitment-page.component.html",
  styleUrls: ["./edit-commitment-page.component.css"],
  selector: "app-edit-commitment-page"
})
export class EditCommitmentPageComponent { 
  constructor(
    private _commitmentService: CommitmentService,
    private _router: Router
  ) { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public handleSaveClick() {
    var commitment = new Commitment();

    commitment.name = this.form.value.name;
    commitment.description = this.form.value.description;

    this._commitmentService.save({ commitment })
      .pipe(takeUntil(this.onDestroy),tap(() => this._router.navigateByUrl("/")))
      .subscribe();
  }
  
  public handleCancelClick() {

  }
  
  public form = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
  });
}
