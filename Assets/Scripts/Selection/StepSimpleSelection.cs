using System.Collections.Generic;
using UnityEngine;

public class StepSimpleSelection : StepSelection {
    private void Awake() {
        this.enabled = false;

        selectTile = Instantiate(Prefabs.instance.selectTile);
        hideObj(selectTile);
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int gridPoint = getGridFromHit(hitPlace);
            showObjOnGrid(selectTile, gridPoint);

            updateAlphaStatus(gridPoint); // Хочеца заменить

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
        if (GameManager.instance.getPieceTypeByGrid(gridPoint) == PieceType.King && (gridPoint == new Vector2Int(4, 0) || gridPoint == new Vector2Int(4, 7))) {
            Castling goTo = GetComponent<Castling>();
            goTo.Activate();
        }

        ActiveSelection(gridPoint, false);

        GameManager.instance.selectSimplePieceAtGrid(startGridPoint);
    }
    public void Disactivate() {
        if (!this.enabled)
            return;

        Castling goTo = GetComponent<Castling>();
        goTo.Disactivate();

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

        Display goTo = GetComponent<Display>();
        goTo.Activate(startGridPoint, gridPoint, false);
    }
    private void Cancel() {
        Disactivate();

        PieceSelection goTo = GetComponent<PieceSelection>();
        goTo.Activate();
    }
}
