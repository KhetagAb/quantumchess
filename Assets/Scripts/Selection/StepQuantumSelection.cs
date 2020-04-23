using System.Collections.Generic;
using UnityEngine;

public class StepQuantumSelection : StepSelection {
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
            Display.instance.setSelectorAtGrid(gridPoint, GameManager.instance.getPieceAtGrid(startGridPoint));

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
                    Cancel(gridPoint);
                }
            }
        } else {
            Display.instance.setSelectorAtGrid(null);
        }
    }

    private void selectMidTile(Vector2Int gridPoint) {
        midGridPoint = gridPoint;
        Display.instance.setPermPieceAtGrid(GameManager.instance.getPieceAtGrid(startGridPoint), (Vector2Int) midGridPoint, true, true);

        hideAllowedGrids();
        showAllowedGrids(startGridPoint, midGridPoint, true);
    }
    private void deselectMidTile() {
        Display.instance.delAlphaPieceAtGrid((Vector2Int) midGridPoint);
        midGridPoint = null;

        hideAllowedGrids();
        showAllowedGrids(startGridPoint, midGridPoint, true);
    }

    public void Activate(Vector2Int gridPoint) {
        midGridPoint = null;

        ActiveSelection(gridPoint, true);

        Display.instance.selectQuantumPieceAtGrid(startGridPoint);
    }
    private void Disactivate(Vector2Int? finishGridPoint = null) {
        if (!this.enabled)
            return;

        DisactiveSelection(finishGridPoint);
    }

    private void ExitToStep(Vector2Int gridPoint) {
        Disactivate(gridPoint);

        GameManager.instance.quantumMove(startGridPoint, (Vector2Int) midGridPoint, gridPoint);

        Display goTo = GetComponent<Display>();
        goTo.Activate(startGridPoint, gridPoint, true);
    }
    private void Cancel(Vector2Int gridPoint) {
        Disactivate(gridPoint);

        Display.instance.selector = null;

        StepSimpleSelection goTo = GetComponent<StepSimpleSelection>();
        goTo.Activate(startGridPoint);
    }
}