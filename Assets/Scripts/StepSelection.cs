using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSelection : Selection {
    protected Vector2Int startGridPoint;

    protected List<Vector2Int> allowedGrids;
    protected List<GameObject> allowedGridsObjects;

    protected GameObject alphaPiece;

    // Не очень нравится.
    protected void updateAlphaStatus(Vector2Int curGridPoint, Vector2Int? midGridPoint = null) {
        if (allowedGrids.Contains(curGridPoint)) {
            showAllTiles(midGridPoint);

            setActiveTileInGrid(curGridPoint, false, midGridPoint);

            if (getPieceIDAtGrid(curGridPoint) == null)
                showObjOnGrid(alphaPiece, curGridPoint);
        } else {
            showAllTiles(midGridPoint);
        }
    }

    protected List<GameObject> getObjectForAllowedGrids(List<Vector2Int> allowedGrids) {
        List<GameObject> allowedGridsObjects = new List<GameObject>();

        foreach (Vector2Int curGrid in allowedGrids) {
            GameObject tempPrefab = PrefabIndexing.instance.getCorrectTile(curGrid);
            allowedGridsObjects.Add(Instantiate(tempPrefab, Geometry.PointFromGrid(curGrid), Quaternion.identity, gameObject.transform));
        }

        return allowedGridsObjects;
    }
    protected List<Vector2Int> getAllowedGrids(Vector2Int startGridPoint, Vector2Int? midGridPoint, bool isQuant) {
        if (midGridPoint == null)
            return GameManager.instance.getAllowedGridsInStep(startGridPoint, isQuant);
        else
            return GameManager.instance.getAllowedGridsInMidStep(startGridPoint, (Vector2Int) midGridPoint);
    }

    protected void showAllowedGrids(Vector2Int startGridPoint, Vector2Int? midGridPoint, bool isQuant) {
        allowedGrids = getAllowedGrids(startGridPoint, midGridPoint, isQuant);
        allowedGridsObjects = getObjectForAllowedGrids(allowedGrids);
    }
    protected void hideAllowedGrids() {
        foreach (GameObject toDelete in allowedGridsObjects)
            Destroy(toDelete);
        allowedGridsObjects.Clear();
        allowedGrids.Clear();
    }

    protected void ActiveSelection(Vector2Int gridPoint, bool isQuant) {
        startGridPoint = gridPoint;

        alphaPiece = Instantiate(PrefabIndexing.getPrefabAlphaByID((int) getPieceIDAtGrid(startGridPoint)));
        hideObj(alphaPiece);

        showAllowedGrids(startGridPoint, null, isQuant);

        this.enabled = true;
    }
    protected void DisactiveSelection() {
        hideObj(selectTile);

        Destroy(alphaPiece);
        alphaPiece = null;

        GameManager.instance.deselectPieceAtGrid(startGridPoint);
        hideAllowedGrids();

        this.enabled = false;
    }
    
    // не очень нравится
    protected void showAllTiles(Vector2Int? except = null) {
        hideObj(alphaPiece);

        for (int i = 0; i < allowedGrids.Count; i++) {
            if (allowedGrids[i] != except)
                allowedGridsObjects[i].SetActive(true);
        }
    }
    protected void setActiveTileInGrid(Vector2Int? gridPoint, bool status, Vector2Int? except = null) {
        if (gridPoint == null || (gridPoint != null  && gridPoint == except))
            return;

        allowedGridsObjects[allowedGrids.IndexOf((Vector2Int) gridPoint)].SetActive(status);
    }

    protected PieceType? getPieceTypeAtGrid(Vector2Int gridPoint) {
        int? ID = GameManager.instance.getPieceIDAtGrid(gridPoint);

        if (ID == null)
            return null;

        return GameManager.instance.getPieceTypeByID((int) ID);
    }
}
