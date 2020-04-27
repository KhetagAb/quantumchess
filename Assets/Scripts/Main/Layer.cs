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
    public Layer(Layer layer) : base(layer) {
        weight = layer.weight;
        pieces = new Piece[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++)
                pieces[i, j] = layer.pieces[i, j];
        }

        castleInstance();
    }

    // ===================================================[FUNCTIONAL] 
    public static bool isCorrectGrid(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;
        return (0 <= col && col <= 7 && 0 <= row && row <= 7);
    }

    public bool isFreedGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return false;

        return getPieceAtGrid(gridPoint) == null;
    }

    // ===================================================[LAYER] 
    public Piece getPieceAtGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return null;

        return pieces[gridPoint.x, gridPoint.y];
    }
    public bool isFriendlyPieceAtGrid(Piece piece, Vector2Int gridPoint) {
        Piece anotherPiece = getPieceAtGrid(gridPoint);
        return piece.isFriendlyPiece(anotherPiece);
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
    private List<Vector2Int> getMoveLocationsInLayer(Piece piece, Vector2Int fromGridPoint, bool isQuant) {
        if (piece == null)
            return new List<Vector2Int>();

        List<Vector2Int> allowedGridsInLayerInStep = piece.getMoveLocations(this, fromGridPoint);
        allowedGridsInLayerInStep.RemoveAll(grid => !isCorrectGrid(grid) ||
                                                     isFriendlyPieceAtGrid(piece, grid));
        if (isQuant)
            allowedGridsInLayerInStep.RemoveAll(grid => getPieceAtGrid(grid) != null);

        return allowedGridsInLayerInStep;
    }
    public List<Vector2Int> getMoveLocationsInLayerInStep(Piece piece, Vector2Int gridPoint, bool isQuant) {
        return getMoveLocationsInLayer(piece, gridPoint, isQuant);
    }
    public List<Vector2Int> getMoveLocationsInLayerInMid(Piece piece, Vector2Int startGridPoint, Vector2Int midGridPoint) {
        if (piece == null || getPieceAtGrid(startGridPoint) == null)
            return new List<Vector2Int>();

        List<Vector2Int> allowedGridsInLayerInMid = getMoveLocationsInLayer(piece, midGridPoint, true);
        Commn.AddLocation(midGridPoint, allowedGridsInLayerInMid);

        return allowedGridsInLayerInMid;
    }

    // ===================================================[LEGACY OF MOVE LOCATIONS] 
    public bool isLayerLegalInStep(Piece piece, Vector2Int startGridPoint, Vector2Int finishGridPoint, bool isQuant) {
        return getMoveLocationsInLayerInStep(piece, startGridPoint, isQuant).Contains(finishGridPoint);
    }
    public bool isLayerLegalInMid(Piece piece, Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        List<Vector2Int> gridPoints = getMoveLocationsInLayerInMid(piece, startGridPoint, midGridPoint);
        return gridPoints.Contains(midGridPoint) && gridPoints.Contains(finishGridPoint);
    }
}
