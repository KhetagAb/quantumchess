using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSStepSelection : StepAndPieceSelection {
    protected Piece startPiece;
    protected Vector2Int startGridPoint;

    protected List<Vector2Int> allowedGrids;

    protected List<Vector2Int> getAllowedGrids(Vector2Int startGridPoint, Vector2Int? midGridPoint, bool isQuant) {
        if (midGridPoint == null)
            return Step.instance.getMoveLocations(startGridPoint, isQuant);
        else
            return Step.instance.getMoveLocations(startGridPoint, (Vector2Int) midGridPoint);
    }

    protected void showAllowedGrids(Vector2Int startGridPoint, Vector2Int? midGridPoint, bool isQuant) {
        allowedGrids = getAllowedGrids(startGridPoint, midGridPoint, isQuant);
        Display.instance.setTiles(allowedGrids);
    }
    protected void hideAllowedGrids() {
        Display.instance.delTiles(allowedGrids);
        allowedGrids.Clear();
    }

    protected void ActiveSelection(Vector2Int gridPoint, bool isQuant) {
        startGridPoint = gridPoint;
        startPiece = Step.instance.getPieceAtGrid(startGridPoint);

        showAllowedGrids(startGridPoint, null, isQuant);

        this.enabled = true;
    }
    protected void DisactiveSelection() {
        Display.instance.deselectPieceAtGrid(startGridPoint);
        Display.instance.delAllNotPermPieces();

        startPiece = null;

        hideAllowedGrids();

        this.enabled = false;
    }
}
