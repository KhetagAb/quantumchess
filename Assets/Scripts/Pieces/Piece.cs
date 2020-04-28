using System.Collections.Generic;
using UnityEngine;

public enum PieceType {King = 0, Queen = 1, Bishop = 2, Knight = 3, Rook = 4, Pawn = 5};

public abstract class Piece {
    public abstract PieceType typeOfPiece { get; protected set; }
    public abstract PlayerColor colorOfPiece { get; protected set; }
    public abstract List<Vector2Int> getMoveLocations(Layer layer, Vector2Int gridPoint);
    protected abstract GameObject getClearPrefab();

    public GameObject getPrefab(bool isAlpha) {
        GameObject temp = getClearPrefab();
        Material toSet = isAlpha ? Prefabs.instance.getAlpha(colorOfPiece) : Prefabs.instance.getDefault(colorOfPiece);

        foreach (MeshRenderer mesh in temp.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;

        return temp;
    }

    public bool isFriendlyPiece(Piece piece) {
        if (piece == null)
            return false;

        return colorOfPiece == piece.colorOfPiece;
    }

    protected static Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected static Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};
    protected static Vector2Int[] KnightDirections = {new Vector2Int(1,2), new Vector2Int(2, 1),
        new Vector2Int(1, -2), new Vector2Int(-2, 1),
        new Vector2Int(2, -1), new Vector2Int(-1, 2),
        new Vector2Int(-2, -1), new Vector2Int(-1, -2)};

}
