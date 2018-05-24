import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { ColDef, GridApi } from "ag-grid";
import { ToDoService } from "./to-do.service";
import { Observable } from "rxjs";
import { ToDo } from "./to-do.model";
import { EditToDoOverlayComponent } from "./edit-to-do-overlay.component";
import { map, takeUntil } from "rxjs/operators";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditToDoOverlay } from "./edit-to-do-overlay";

@Component({
  templateUrl: "./to-dos-page.component.html",
  styleUrls: ["./to-dos-page.component.css"],
  selector: "app-to-dos-page"
})
export class ToDosPageComponent { 
  constructor(
    private _editToDoOverlay: EditToDoOverlay,
    private _toDoService: ToDoService) {

    this.handleRemoveToDoCellClick = this.handleRemoveToDoCellClick.bind(this);
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name" },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemoveToDoCellClick($event) }
  ];

  public frameworkComponents: any = {
    deleteRenderer: DeleteCellComponent
  };

  private _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
  }

  public toDos$: BehaviorSubject<Array<ToDo>>;

  public handleFabButtonClick() {
    const overlayRefWrapper = this._editToDoOverlay.create();
    
    overlayRefWrapper.afterClosed()
      .pipe(map(toDo => this.addOrUpdate(toDo)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleRemoveToDoCellClick($event) {

  }

  public addOrUpdate(toDo:ToDo) {
    if (!toDo) return;

    let toDos = [...this.toDos$.value];
    const i = toDos.findIndex((t) => t.toDoId == toDo.toDoId);

    if (i == null) {
      toDos.push(toDo);
    } else {
      toDos[i] = toDo;
    }

    this.toDos$.next(toDos);
  }
}
