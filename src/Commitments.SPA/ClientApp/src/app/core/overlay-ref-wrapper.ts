import { OverlayRef } from '@angular/cdk/overlay';
import { Subject, Observable } from 'rxjs';

export class OverlayRefWrapper {
  constructor(private overlayRef: OverlayRef) {}

  public close(data: any = null): void {    
    this.overlayRef.dispose();
    this.results.next(data);
  }
  
  public results = new Subject<any>();

  public data: any;
  
}
