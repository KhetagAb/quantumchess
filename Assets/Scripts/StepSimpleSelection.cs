using System.Collections.Generic;
using UnityEngine;

public class StepSimpleSelection : StepSelection {
    private void Awake() {
        this.enabled = false;

        selectTile = Instantiate(PrefabIndexing.instance.prefSelectTile);
        hideObj(selectTile);
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int gridPoint = getGridFromHit(hitPlace);
            showObjOnGrid(selectTile, gridPoint);

            updateAlphaStatus(gridPoint);

            if (Input.GetMouseButtonDown(0)) {
                if (gridPoint == startGridPoint && getPieceTypeAtGrid(startGridPoint) != PieceType.Pawn)
                    ExitToQuantum();
                else if (allowedGrids.Contains(gridPoint))
                    ExitToStep(gridPoint);
            } else if (Input.GetMouseButtonDown(2)) {
                Cancel();
            }
        } else {
            showAllTiles();
            hideObj(selectTile);
        }
    }

    public void Activate(Vector2Int gridPoint) {
        ActiveSelection(gridPoint, false);

        GameManager.instance.selectSimplePieceAtGrid(startGridPoint);
    }

    public void Disactivate() {
        if (!this.enabled)
            return;

        DisactiveSelection();
    }

    private void ExitToQuantum() {
        Disactivate();

        StepQuantumSelection goTo = GetComponent<StepQuantumSelection>();
        goTo.Activate(startGridPoint);
    }

    private void ExitToStep(Vector2Int gridPoint) {
        Disactivate();

        GameManager.instance.simpleMove(startGridPoint, gridPoint);

        StepAndBoardDisplay goTo = GetComponent<StepAndBoardDisplay>();
        goTo.EnterState(startGridPoint, gridPoint, false);
    }

    private void Cancel() {
        Disactivate();

        PieceSelection goTo = GetComponent<PieceSelection>();
        goTo.Activate();
    }
}
