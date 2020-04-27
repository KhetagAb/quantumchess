using System.Collections.Generic;
using UnityEngine;

public class QuantumSelection : QSStepSelection {
    [SerializeField] private GameObject WaringWindow;

    private Vector2Int? midGridPoint;

    private void Awake() {
        this.enabled = false;
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int gridPoint = getGridFromHit(hitPlace);
            Display.instance.setSelector(gridPoint, startPiece);

            if (midGridPoint != null) {
                if (Input.GetMouseButtonDown(0)) {
                    if (allowedGrids.Contains(gridPoint))
                        ExitToStep(gridPoint);
                } else if (Input.GetMouseButtonDown(2)) {
                    deselectMidTile();
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    if (allowedGrids.Contains(gridPoint))
                        selectMidTile(gridPoint);
                } else if (Input.GetMouseButtonDown(2)) {
                    Cancel();
                }
            }
        } else {
            Display.instance.setSelector(null);
        }
    }

    private void selectMidTile(Vector2Int gridPoint) {
        midGridPoint = gridPoint;
        Display.instance.changeTypeToUntilPiece((Vector2Int) midGridPoint);

        hideAllowedGrids();
        showAllowedGrids(startGridPoint, midGridPoint, true);
    }
    private void deselectMidTile() {
        Display.instance.delAllNotPermPieces();

        midGridPoint = null;
        hideAllowedGrids();
        showAllowedGrids(startGridPoint, midGridPoint, true);

        Display.instance.setSelector(Display.instance.selector, startPiece, false);
    }

    public void Activate(Vector2Int gridPoint) {
        midGridPoint = null;

        ActiveSelection(gridPoint, true);

        Display.instance.selectQuantumPieceAtGrid(startGridPoint);
    }
    private void Disactivate() {
        if (!this.enabled)
            return;

        DisactiveSelection();
    }

    private void ExitToStep(Vector2Int gridPoint) {
        if (!Step.instance.isQuantumMovePos(startGridPoint, (Vector2Int) midGridPoint, gridPoint)) {
            this.enabled = false;
            WaringWindow.SetActive(true);
            return;
        }

        Disactivate();

        Step.instance.QuantumMove(startGridPoint, (Vector2Int) midGridPoint, gridPoint);
    }
    private void Cancel() {
        Disactivate();

        Display.instance.selector = null;

        SimpleSelection goTo = GetComponent<SimpleSelection>();
        goTo.Activate(startGridPoint);
    }
}