using System.Collections.Generic;
using UnityEngine;

public class Layer : CastleInLayer {
    public int weight;
    public int?[,] pieces;
    
    private void castleInstance() {
        childLayer = this;
    }

    public Layer() {
        weight = 1;
        pieces = new int?[8, 8]; // WhiteShort, WhiteLong, BlackShort, BlackLong;

        castleInstance();
    }
    public Layer(Layer layer): base(layer) {
        weight = layer.weight;
        pieces = new int?[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++)
                pieces[i, j] = layer.pieces[i, j];
        }

        castleInstance();
    }

    // ===================================================[FUNCTIONAL] 
    protected static Player getCurrentPlayer() {
        return GameManager.instance.currentPlayer;
    }
    public static bool isCorrectGrid(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;
        return (0 <= col && col <= 7 && 0 <= row && row <= 7);
    }
    public static bool isWhite(int ID) {
        return !((getCurrentPlayer().name == PlayerType.White) ^ (getCurrentPlayer().playersPieces.Contains(ID)));
    }

    // ===================================================[LAYER] 
    public int? getPieceIDAtGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return null;

        return pieces[gridPoint.x, gridPoint.y];
    }
    public bool isFriendlyPieceAtGrid(Vector2Int gridPoint) {
        int? id = getPieceIDAtGrid(gridPoint);
        return id != null && getCurrentPlayer().playersPieces.Contains((int) id);
    }
    public bool isAllowedGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return false;

        return (getPieceIDAtGrid(gridPoint) == null);
    }

    // ===================================================[MOVE] 
    public void moveFromTo(Vector2Int startGridPoint, Vector2Int finishGrdiPoint) {
        updateCastleUntochness(startGridPoint, finishGrdiPoint);
        setFromTo(startGridPoint, finishGrdiPoint);
    }
    public void setFromTo(Vector2Int startGridPoint, Vector2Int finishGrdiPoint) {
        int cols = startGridPoint.x, rows = startGridPoint.y;
        int colf = finishGrdiPoint.x, rowf = finishGrdiPoint.y;
        pieces[colf, rowf] = pieces[cols, rows];
        pieces[cols, rows] = null;
    }

    // ===================================================[MOVE LOCATIONS] 
    private List<Vector2Int> getMoveLocationsInLayer(int ID, Vector2Int gridPoint, bool isQuant) {
        Piece needClass = PrefabIndexing.getPieceClass(ID);

        List<Vector2Int> allowedGridsInLayerInStep = needClass.getMoveLocations(this, isWhite(ID), gridPoint);
        allowedGridsInLayerInStep.RemoveAll(grid => !isCorrectGrid(grid) ||
                                                     isFriendlyPieceAtGrid(grid));
        if (isQuant)
            allowedGridsInLayerInStep.RemoveAll(grid => getPieceIDAtGrid(grid) != null);

        return allowedGridsInLayerInStep;
    }
    public List<Vector2Int> getMoveLocationsInLayerInStep(int ID, Vector2Int gridPoint, bool isQuant) {
        List<Vector2Int> allowedGridsInLayerInStep = new List<Vector2Int>();

        if (getPieceIDAtGrid(gridPoint) == null)
            return allowedGridsInLayerInStep;

        return getMoveLocationsInLayer(ID, gridPoint, isQuant);
    }
    public List<Vector2Int> getMoveLocationsInLayerInMid(int ID, Vector2Int startGridPoint, Vector2Int midGridPoint) {
        List<Vector2Int> allowedGridsInLayerInMid = new List<Vector2Int>();

        if (getPieceIDAtGrid(startGridPoint) == null)
            return allowedGridsInLayerInMid;

        allowedGridsInLayerInMid = getMoveLocationsInLayer(ID, midGridPoint, true);
        Piece.AddLocation(midGridPoint, allowedGridsInLayerInMid);

        return allowedGridsInLayerInMid;
    }

    // ===================================================[LEGACY OF MOVE LOCATIONS] 
    public bool isLayerLegalInStep(int ID, Vector2Int startGridPoint, Vector2Int finishGridPoint, bool isQuant) {
        return getMoveLocationsInLayerInStep(ID, startGridPoint, isQuant).Contains(finishGridPoint);
    }
    public bool isLayerLegalInMid(int ID, Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        List<Vector2Int> gridPoints = getMoveLocationsInLayerInMid(ID, startGridPoint, midGridPoint);
        return gridPoints.Contains(midGridPoint) && gridPoints.Contains(finishGridPoint);
    }
}
