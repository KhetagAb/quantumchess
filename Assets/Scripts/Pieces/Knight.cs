using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {
    public override PieceType typeOfPiece { get; protected set; }

    public Knight(PieceType typeOfPiece) {
        this.typeOfPiece = typeOfPiece;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, bool isWhite, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int direction in KnightDirections)
            locations.Add(new Vector2Int(direction.x + gridPoint.x, direction.y + gridPoint.y));

        return locations;
    }
}
