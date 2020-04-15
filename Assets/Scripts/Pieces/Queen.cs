using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {
    public override PieceType typeOfPiece { get; protected set; }

    public Queen(PieceType typeOfPiece) {
        this.typeOfPiece = typeOfPiece;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, bool isWhite, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(RookDirections);
        directions.AddRange(BishopDirections);

        foreach (Vector2Int direction in directions) {
            Vector2Int tempGridPoint = gridPoint;

            for (int i = 0; i < 8; i++) {
                tempGridPoint += direction;
                locations.Add(tempGridPoint);
                if (!layer.isAllowedGrid(tempGridPoint))
                    break;
            }
        }
        return locations;
    }
}
