// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { OverlayRef } from '@angular/cdk/overlay';
import { Subject, Observable } from 'rxjs';

export class OverlayRefWrapper {
  constructor(private overlayRef: OverlayRef) {}

  public close(data: any = null): void {    
    this.overlayRef.dispose();
    this._afterClosed.next(data);
  }

  public afterClosed(): Observable<any> {
    return this._afterClosed;
  }

  private readonly _afterClosed = new Subject<any>();

  public data: any = {};
  
}

