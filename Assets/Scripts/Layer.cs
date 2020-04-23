using System.Collections.Generic;
using UnityEngine;

public class Layer : CastleInLayer {
    public int weight;
    public Piece[,] pieces;
    
    private void castleInstance() {
        childLayer = this;
    }

    public Layer() {
        weight = 1;
        pieces = new Piece[8, 8];

        castleInstance();
    }
    public Layer(Layer layer): base(layer) {
        weight = layer.weight;
        pieces = new Piece[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++)
                pieces[i, j] = layer.pieces[i, j];
        }

        castleInstance();
    }

    // ===================================================[FUNCTIONAL] 
    protected static Player getCurrentPlayer() {
        return GameManager.instance.curPlayer;
    }
    public static bool isCorrectGrid(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;
        return (0 <= col && col <= 7 && 0 <= row && row <= 7);
    }
    public static bool isWhite(Piece piece) {
        return (piece.colorOfPiece == PlayerColor.White);
    }

    // ===================================================[LAYER] 
    public Piece getPieceAtGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return null;

        return pieces[gridPoint.x, gridPoint.y];
    }
    public bool isFriendlyPieceAtGrid(Vector2Int gridPoint) {
        Piece piece = getPieceAtGrid(gridPoint);
        return piece != null && getCurrentPlayer().color == piece.colorOfPiece;
    }
    public bool isAllowedGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return false;

        return (getPieceAtGrid(gridPoint) == null);
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
    private List<Vector2Int> getMoveLocationsInLayer(Vector2Int pieceGridPoint, Vector2Int fromGridPoint, bool isQuant) {
        Piece piece = getPieceAtGrid(pieceGridPoint);

        if (piece == null)
            return new List<Vector2Int>();

        List<Vector2Int> allowedGridsInLayerInStep = piece.getMoveLocations(this, fromGridPoint);
        allowedGridsInLayerInStep.RemoveAll(grid => !isCorrectGrid(grid) ||
                                                     isFriendlyPieceAtGrid(grid));
        if (isQuant)
            allowedGridsInLayerInStep.RemoveAll(grid => getPieceAtGrid(grid) != null);

        return allowedGridsInLayerInStep;
    }
    public List<Vector2Int> getMoveLocationsInLayerInStep(Vector2Int gridPoint, bool isQuant) {
        return getMoveLocationsInLayer(gridPoint, gridPoint, isQuant);
    }
    public List<Vector2Int> getMoveLocationsInLayerInMid(Vector2Int startGridPoint, Vector2Int midGridPoint) {
        List<Vector2Int> allowedGridsInLayerInMid = new List<Vector2Int>();

        if (getPieceAtGrid(startGridPoint) == null)
            return allowedGridsInLayerInMid;

        allowedGridsInLayerInMid = getMoveLocationsInLayer(startGridPoint, midGridPoint, true);
        Piece.AddLocation(midGridPoint, allowedGridsInLayerInMid);

        return allowedGridsInLayerInMid;
    }

    // ===================================================[LEGACY OF MOVE LOCATIONS] 
    public bool isLayerLegalInStep(Vector2Int startGridPoint, Vector2Int finishGridPoint, bool isQuant) {
        return getMoveLocationsInLayerInStep(startGridPoint, isQuant).Contains(finishGridPoint);
    }
    public bool isLayerLegalInMid(Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        List<Vector2Int> gridPoints = getMoveLocationsInLayerInMid(startGridPoint, midGridPoint);
        return gridPoints.Contains(midGridPoint) && gridPoints.Contains(finishGridPoint);
    }
}
