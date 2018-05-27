import { Component, ElementRef, Inject } from '@angular/core';
import { ProfileService } from './profiles/profile.service';
import { AppStore } from './app-store';
import { map, switchMap } from 'rxjs/operators';
import { baseUrl } from './core/constants';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.css'],
  selector: 'app-master-page'
})
export class MasterPageComponent {
  constructor(
    @Inject(baseUrl)private _baseUrl:string,
    private _elementRef: ElementRef,
    private _profileService: ProfileService,
    private _appStore: AppStore,
    private _router: Router
  ) {

  }

  public ngOnInit() {
    this._profileService.current()
      .pipe(
        map(x => this._appStore.currentProfile$.next(x)),
        switchMap(x => this._appStore.currentProfile$),
        map(x => {
          this._setCustomProperty("--background-image-url", `url(${this._baseUrl}${x.avatarUrl})`);          
      })
    )
      .subscribe();
  }

  protected _setCustomProperty(key: string, value: any) {
    this._elementRef.nativeElement.style.setProperty(key, value)
  }

  public get profileName$(): Observable<string> {
    return this._appStore.currentProfile$.pipe(map(x => x.name));
  }

  public onProfileNameClick() {
    this._router.navigateByUrl("/my-profile");
  }
}
