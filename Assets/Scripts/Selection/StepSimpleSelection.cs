using System.Collections.Generic;
using UnityEngine;

public class StepSimpleSelection : StepSelection {
    private void Awake() {
        this.enabled = false;
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int  gridPoint = getGridFromHit(hitPlace);
            Display.instance.setSelectorAtGrid(gridPoint, GameManager.instance.getPieceAtGrid(startGridPoint));

            if (Input.GetMouseButtonDown(0)) {
                if (gridPoint == startGridPoint && GameManager.instance.getPieceTypeAtGrid(gridPoint) != PieceType.Pawn)
                    ExitToQuantum(gridPoint);
                else if (allowedGrids.Contains(gridPoint))
                    ExitToStep(gridPoint);
            } else if (Input.GetMouseButtonDown(2)) {
                Cancel(gridPoint);
            }
        } else {
            Display.instance.setSelectorAtGrid(null);
        }
    }

    public void Activate(Vector2Int gridPoint) {
        if (GameManager.instance.getPieceTypeAtGrid(gridPoint) == PieceType.King && (gridPoint == new Vector2Int(4, 0) || gridPoint == new Vector2Int(4, 7))) {
            Castling goTo = GetComponent<Castling>();
            goTo.Activate();
        }

        ActiveSelection(gridPoint, false);

        Display.instance.selectSimplePieceAtGrid(startGridPoint);
    }
    public void Disactivate(Vector2Int? finishGridPoint = null) {
        if (!this.enabled)
            return;

        Castling goTo = GetComponent<Castling>();
        goTo.Disactivate();

        DisactiveSelection(finishGridPoint);
    }

    private void ExitToQuantum(Vector2Int gridPoint) {
        Disactivate(gridPoint);

        StepQuantumSelection goTo = GetComponent<StepQuantumSelection>();
        goTo.Activate(startGridPoint);
    }
    private void ExitToStep(Vector2Int gridPoint) {
        Disactivate(gridPoint);

        GameManager.instance.simpleMove(startGridPoint, gridPoint);

        Display goTo = GetComponent<Display>();
        goTo.Activate(startGridPoint, gridPoint, false);
    }
    private void Cancel(Vector2Int gridPoint) {
        Disactivate(gridPoint);

        PieceSelection goTo = GetComponent<PieceSelection>();
        goTo.Activate();
    }
}
