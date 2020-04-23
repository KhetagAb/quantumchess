using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    public override PieceType typeOfPiece { get; protected set; }
    public override PlayerColor colorOfPiece { get; protected set; }

    protected override GameObject getClearPrefab() {
        return Prefabs.instance.prefKing;
    }

    public King(PlayerColor color) {
        typeOfPiece = PieceType.King;
        colorOfPiece = color;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(RookDirections);
        directions.AddRange(BishopDirections);

        foreach (Vector2Int direction in directions)
            locations.Add(new Vector2Int(direction.x + gridPoint.x, direction.y + gridPoint.y));

        return locations;
    }
}
