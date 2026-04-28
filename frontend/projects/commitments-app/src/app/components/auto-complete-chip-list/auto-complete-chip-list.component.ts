// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, forwardRef, input, output, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { startWith, map } from 'rxjs';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatAutocompleteSelectedEvent, MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-auto-complete-chip-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatIconModule
  ],
  templateUrl: './auto-complete-chip-list.component.html',
  styleUrls: ['./auto-complete-chip-list.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AutoCompleteChipListComponent),
      multi: true
    }
  ]
})
export class AutoCompleteChipListComponent implements ControlValueAccessor {
  writeValue(obj: any): void {
    obj = this.addItems.value;
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  public onTouchedCallback: () => void = () => {};

  public onChangeCallback: (_: any) => void = () => {};

  public readonly onChipClick = output<{ item: any }>();

  public readonly chipInput = viewChild<MatInput>('chipInput');
  public readonly selectedItems = input<string[]>([]);
  public readonly items = input<Item[]>([]);

  public addItems: FormControl = new FormControl();
  public filteredItems = toSignal(
    this.addItems.valueChanges.pipe(
      startWith(''),
      map(item => (item ? this.filterItems(item.toString()) : this.items().slice()))
    ),
    { initialValue: [] as Item[] }
  );

  separatorKeysCodes = [ENTER, COMMA];

  private _localSelected: string[] = [];

  ngOnInit() {
    this._localSelected = [...(this.selectedItems() ?? [])];
  }

  get itemsData(): string[] {
    return this._localSelected;
  }

  set itemsData(v: string[]) {
    this._localSelected = v;
  }

  filterItems(itemName: string) {
    return this.items().filter(item => item.name.toLowerCase().indexOf(itemName.toLowerCase()) === 0);
  }

  onRemoveItems(itemName: string): void {
    this._localSelected = this._localSelected.filter(name => name !== itemName);
    this.chipInput()['nativeElement'].blur();
    this.onChangeCallback(this._localSelected);
  }

  onAddItems(event: MatAutocompleteSelectedEvent) {
    const t: Item = event.option.value;

    if (this._localSelected.length === 0) {
      this._localSelected.push(t.name);
    } else {
      const selectLanguageStr = JSON.stringify(this._localSelected);
      if (selectLanguageStr.indexOf(t.name) === -1) {
        this._localSelected.push(t.name);
      }
    }

    this.chipInput()['nativeElement'].blur();
    this.chipInput()['nativeElement'].value = '';
    this.onChangeCallback(this._localSelected);
  }
}

export class Item {
  itemId: number;
  name: string;
  constructor(itemId: number, name: string) {}
}
