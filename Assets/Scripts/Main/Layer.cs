using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : CastleInLayer {
    public int weight;

    public Layer() {
        weight = 1;
        pieces = new Piece[8, 8];

        calRang();
    }
    public Layer(Layer layer) : base(layer) {
        weight = layer.weight;
        pieces = new Piece[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++)
                pieces[i, j] = layer.pieces[i, j];
        }

        calRang();
    }

    // ===================================================[FUNCTIONAL] 
    public bool isFreedGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return false;

        return getPieceAtGrid(gridPoint) == null;
    }

    // ===================================================[LAYER] 
    public bool isFriendlyPieceAtGrid(Piece piece, Vector2Int gridPoint) {
        Piece anotherPiece = getPieceAtGrid(gridPoint);
        return piece.isFriendlyPiece(anotherPiece);
    }

    // ===================================================[MOVE] 
    public void moveFromTo(Step step) {
        setFromTo(step);

        normalizeCastleInLayer();
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
    public bool isLayerLegalInStep(Piece piece, Step step, bool isQuant) {
        return getMoveLocationsInLayerInStep(piece, step.from, isQuant).Contains(step.to);
    }
    public bool isLayerLegalInMid(Piece piece, Step step) {
        List<Vector2Int> gridPoints = getMoveLocationsInLayerInMid(piece, step.from, (Vector2Int) step.mid);
        return gridPoints.Contains((Vector2Int) step.mid) && gridPoints.Contains(step.to);
    }
}
