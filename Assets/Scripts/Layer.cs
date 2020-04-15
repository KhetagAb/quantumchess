using System.Collections.Generic;
using UnityEngine;

public class Layer {
    public int weight;
    public int?[,] pieces;
    public bool[] roques;

    public Layer() {
        weight = 1;
        pieces = new int?[8, 8];
        roques = new bool[] { true, true, true, true }; // WhiteShort, WhiteLong, BlackShort, BlackLong;
    }

    public Layer(Layer layer) {
        weight = layer.weight;
        pieces = new int?[8, 8];
        roques = new bool[4];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++)
                pieces[i, j] = layer.pieces[i, j];
        }

        for (int i = 0; i < roques.Length; i++)
            roques[i] = layer.roques[i];
    }

    private static Player getCurrentPlayer() {
        return GameManager.instance.currentPlayer;
    }
    private static Vector2Int[] rooksRoque = new Vector2Int[] {
        new Vector2Int(7, 0), new Vector2Int(0, 0),
        new Vector2Int(7, 7), new Vector2Int(0, 7) };
    private static Vector2Int[] kingRoque = new Vector2Int[] {
        new Vector2Int(4, 0), new Vector2Int(4, 7) };
    private static List<Vector2Int>[] betweenRoque = {
        new List<Vector2Int>() { new Vector2Int(5, 0), new Vector2Int(6, 0) },                          // 0
        new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0) },    // 1
        new List<Vector2Int>() { new Vector2Int(5, 7), new Vector2Int(6, 7) },                          // 2
        new List<Vector2Int>() { new Vector2Int(1, 7), new Vector2Int(2, 7), new Vector2Int(3, 7) }};   // 3
    public static bool isCorrectGrid(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;
        return (0 <= col && col <= 7 && 0 <= row && row <= 7);
    }
    public static bool isWhite(int ID) {
        return !((getCurrentPlayer().name == PlayerType.White) ^ (getCurrentPlayer().playersPieces.Contains(ID)));
    }

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

    // претендент на костыли *
    // подумать бы над оптимайзом
    public void updateRoqueStatus(PieceType pieceType, Vector2Int startPoint, Vector2Int finishGrid) { 
        for (int i = 0; i < roques.Length; i++)
            roques[i] = roques[i] && (finishGrid != rooksRoque[i]);

        for (int i = 0; i < kingRoque.Length; i++) {
            roques[i] = roques[i] && (finishGrid != kingRoque[i]);
            roques[i + 1] = roques[i + 1] && (finishGrid != kingRoque[i]);
        }

        int playerIndex = ((getCurrentPlayer().name == PlayerType.Black) ? 2 : 0);
        if (pieceType == PieceType.King) {
            roques[playerIndex] = roques[playerIndex + 1] = false;
        } else if (pieceType == PieceType.Rook) {
            if (startPoint == rooksRoque[0] || startPoint == rooksRoque[2])
                roques[playerIndex] = false;
            else if (startPoint == rooksRoque[1] || startPoint == rooksRoque[3])
                roques[playerIndex + 1] = false;
        }
    }

    public bool[] getRoqueStatus() {
        bool[] inCurLayer = new bool[roques.Length];
        for (int i = 0; i < roques.Length; i++) {
            inCurLayer[i] = roques[i];
                
            foreach (Vector2Int grid in betweenRoque[i])
                inCurLayer[i] = inCurLayer[i] && (getPieceIDAtGrid(grid) == null);
        }

        return inCurLayer;
    }

    public void setFromTo(Vector2Int startGridPoint, Vector2Int finishGrdiPoint) {
        int cols = startGridPoint.x, rows = startGridPoint.y;
        int colf = finishGrdiPoint.x, rowf = finishGrdiPoint.y;
        pieces[colf, rowf] = pieces[cols, rows];
        pieces[cols, rows] = null;
    }

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

    public bool isLayerLegalInStep(int ID, Vector2Int startGridPoint, Vector2Int finishGridPoint, bool isQuant) {
        return getMoveLocationsInLayerInStep(ID, startGridPoint, isQuant).Contains(finishGridPoint);
    }

    public bool isLayerLegalInMid(int ID, Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        List<Vector2Int> gridPoints = getMoveLocationsInLayerInMid(ID, startGridPoint, midGridPoint);
        return gridPoints.Contains(midGridPoint) && gridPoints.Contains(finishGridPoint);
    }
}
