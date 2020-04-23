using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSelection : Selection {
    protected Vector2Int startGridPoint;

    protected List<Vector2Int> allowedGrids;

    protected List<Vector2Int> getAllowedGrids(Vector2Int startGridPoint, Vector2Int? midGridPoint, bool isQuant) {
        if (midGridPoint == null)
            return GameManager.instance.getAllowedGridsInStep(startGridPoint, isQuant);
        else
            return GameManager.instance.getAllowedGridsInMidStep(startGridPoint, (Vector2Int) midGridPoint);
    }

    protected void showAllowedGrids(Vector2Int startGridPoint, Vector2Int? midGridPoint, bool isQuant) {
        allowedGrids = getAllowedGrids(startGridPoint, midGridPoint, isQuant);
        Display.instance.setPermTiles(allowedGrids);
    }
    protected void hideAllowedGrids() {
        Display.instance.delPermTiles(allowedGrids);
        allowedGrids.Clear();
    }

    protected void ActiveSelection(Vector2Int gridPoint, bool isQuant) {
        startGridPoint = gridPoint;

        showAllowedGrids(startGridPoint, null, isQuant);

        this.enabled = true;
    }
    protected void DisactiveSelection(Vector2Int? finishGridPoint = null) {
        Display.instance.deselectPieceAtGrid(startGridPoint);

        if (finishGridPoint != null) {
            Display.instance.delAlphaPieceAtGrid((Vector2Int) finishGridPoint);
        }

        hideAllowedGrids();

        this.enabled = false;
    }
}
