using System.Collections.Generic;
using UnityEngine;

public enum PieceType {King, Queen, Bishop, Knight, Rook, Pawn};

public abstract class Piece {
    public abstract PieceType typeOfPiece { get; protected set; }
    public abstract List<Vector2Int> getMoveLocations(Layer layer, bool isWhite, Vector2Int gridPoint);

    protected Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};

    protected Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};

    protected Vector2Int[] KnightDirections = {new Vector2Int(1,2), new Vector2Int(2, 1),
        new Vector2Int(1, -2), new Vector2Int(-2, 1),
        new Vector2Int(2, -1), new Vector2Int(-1, 2),
        new Vector2Int(-2, -1), new Vector2Int(-1, -2)};

    public static void AddLocation(Vector2Int gridPoint, List<Vector2Int> locations) {
        if (!locations.Contains(gridPoint))
            locations.Add(gridPoint);
    }
}
