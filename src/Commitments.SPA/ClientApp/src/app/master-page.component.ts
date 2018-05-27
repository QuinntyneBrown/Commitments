import { Component, ElementRef, Inject } from '@angular/core';
import { ProfileService } from './profiles/profile.service';
import { AppStore } from './app-store';
import { map, switchMap } from 'rxjs/operators';
import { baseUrl } from './core/constants';

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
    private _appStore: AppStore
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

}
