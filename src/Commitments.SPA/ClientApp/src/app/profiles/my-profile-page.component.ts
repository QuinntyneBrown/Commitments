import { Component, Inject } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile.model";
import { takeUntil, map, tap } from "rxjs/operators";
import { LocalStorageService } from "../core/local-storage.service";
import { currentProfileIdKey, baseUrl } from "../core/constants";
import { FormGroup, FormControl } from "@angular/forms";

@Component({
  templateUrl: "./my-profile-page.component.html",
  styleUrls: ["./my-profile-page.component.css"],
  selector: "app-my-profile-page"
})
export class MyProfilePageComponent { 
  constructor(
    @Inject(baseUrl) public baseUrl: string,
    private _profileService: ProfileService,
    private _storage: LocalStorageService
  ) { }

  public ngOnInit() {
    this.profile$ = this._profileService.current()
      .pipe(
        tap(x => this.form.patchValue({
          avatarUrl: x.avatarUrl
        }))
      );
  }

  public handleSaveClick() {
    alert("?");
    this._profileService.saveAvatarUrl(
      {
        avatarUrl: this.form.value.avatarUrl,
        profileId: this.profileId
      })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public profile$: Observable<Profile>;
  
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public get profileId(): number {
    return +this._storage.get({ name: currentProfileIdKey });
  }

  public form: FormGroup = new FormGroup({
    avatarUrl: new FormControl(null,[])
  });
}
