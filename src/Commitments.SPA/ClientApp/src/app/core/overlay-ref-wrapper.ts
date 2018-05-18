import { OverlayRef } from '@angular/cdk/overlay';
import { Subject, Observable } from 'rxjs';

export class OverlayRefWrapper {
  constructor(private overlayRef: OverlayRef) {}

  close(data: any = null): void {    
    this.overlayRef.dispose();
    this.afterClosed.next(data);
  }
  
  public afterClosed = new Subject<any>();
  
  
}
