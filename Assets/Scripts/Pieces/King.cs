using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    public override PieceType typeOfPiece { get; protected set; }

    public King(PieceType typeOfPiece) {
        this.typeOfPiece = typeOfPiece;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, bool isWhite, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(RookDirections);
        directions.AddRange(BishopDirections);

        foreach (Vector2Int direction in directions)
            locations.Add(new Vector2Int(direction.x + gridPoint.x, direction.y + gridPoint.y));

        return locations;
    }
}
