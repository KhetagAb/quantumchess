using System.Collections.Generic;
using UnityEngine;

public class SimpleSelection : QSStepSelection {
    private void Awake() {
        this.enabled = false;
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int  gridPoint = getGridFromHit(hitPlace);
            Display.instance.setSelector(gridPoint, startPiece);

            if (Input.GetMouseButtonDown(0)) {
                if (gridPoint == startGridPoint && getPieceTypeAtGrid(gridPoint) != PieceType.Pawn)
                    ExitToQuantum();
                else if (allowedGrids.Contains(gridPoint))
                    ExitToStep(gridPoint);
            } else if (Input.GetMouseButtonDown(2)) {
                Cancel();
            }
        } else {
            Display.instance.setSelector(null);
        }
    }

    public void Activate(Vector2Int gridPoint) {
        if (getPieceTypeAtGrid(gridPoint) == PieceType.King && (gridPoint == new Vector2Int(4, 0) || gridPoint == new Vector2Int(4, 7))) {
            Castling goTo = GetComponent<Castling>();
            goTo.Activate();
        }

        ActiveSelection(gridPoint, false);

        Display.instance.selectSimplePieceAtGrid(startGridPoint);
    }
    public void Disactivate() {
        if (!this.enabled)
            return;

        Castling goTo = GetComponent<Castling>();
        goTo.Disactivate();

        DisactiveSelection();
    }

    protected PieceType getPieceTypeAtGrid(Vector2Int gridPoint) {
        Piece piece = getPieceAtGrid(gridPoint);

        return piece.typeOfPiece;
    }

    private void ExitToQuantum() {
        Disactivate();

        QuantumSelection goTo = GetComponent<QuantumSelection>();
        goTo.Activate(startGridPoint);
    }
    private void ExitToStep(Vector2Int gridPoint) {
        Disactivate();

        GameManager.instance.SimpleMove(new Step(startGridPoint, gridPoint));
    }
    private void Cancel() {
        Disactivate();

        PieceSelection goTo = GetComponent<PieceSelection>();
        goTo.Activate();
    }
}
