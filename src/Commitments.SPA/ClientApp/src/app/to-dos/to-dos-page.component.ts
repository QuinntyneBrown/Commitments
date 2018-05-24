import { Component, Injector, ComponentRef } from "@angular/core";
import { Subject } from "rxjs";
import { ColDef, GridApi } from "ag-grid";
import { ToDoService } from "./to-do.service";
import { Observable } from "rxjs";
import { ToDo } from "./to-do.model";
import { EditToDoOverlayComponent } from "./edit-to-do-overlay.component";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { map, takeUntil } from "rxjs/operators";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { DeleteCellComponent } from "../shared/delete-cell.component";

@Component({
  templateUrl: "./to-dos-page.component.html",
  styleUrls: ["./to-dos-page.component.css"],
  selector: "app-to-dos-page"
})
export class ToDosPageComponent { 
  constructor(
    private _injector: Injector,
    private _overlayRefProvider: OverlayRefProvider,
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

  public toDos$: Observable<Array<ToDo>>;

  public handleEditAddressClick() {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);

    overlayRefWrapper.afterClosed
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleRemoveToDoCellClick($event) {

  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    const injectionTokens = new WeakMap();
    injectionTokens.set(OverlayRefWrapper, overlayRefWrapper);
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(EditToDoOverlayComponent, null, injector);
    const overlayPortalRef: ComponentRef<EditToDoOverlayComponent> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}
