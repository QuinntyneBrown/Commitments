// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef, forwardRef, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

declare var Quill: any;

@Component({
  selector: 'app-quill-text-editor',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quill-text-editor.component.html',
  styleUrls: ['./quill-text-editor.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => QuillTextEditorComponent),
      multi: true
    }
  ]
})
export class QuillTextEditorComponent implements ControlValueAccessor {
  private readonly _elementRef = inject(ElementRef);

  constructor() {
    this.onTextChanged = this.onTextChanged.bind(this);
  }

  public writeValue(value: any): void {
    if (this.qlEditorNativeElement && value) this.qlEditorNativeElement.innerHTML = value;
  }

  public registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  public setDisabledState?(isDisabled: boolean): void {}

  public onTouchedCallback: () => void = () => {};

  public onChangeCallback: (_: any) => void = () => {};

  public ngAfterViewInit() {
    this._quill = new Quill(this.nativeElement, {
      theme: 'bubble',
      placeholder: this.editorPlaceholder
    });

    this._quill.on('text-change', this.onTextChanged);
  }

  public onTextChanged(delta, oldDelta, source) {
    this.onChangeCallback(this.qlEditorNativeElement.innerHTML);
  }

  @Input() public editorPlaceholder: string;

  public get nativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('.editor') as HTMLElement;
  }

  public get qlEditorNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('.ql-editor') as HTMLElement;
  }

  private _quill: any;
}
