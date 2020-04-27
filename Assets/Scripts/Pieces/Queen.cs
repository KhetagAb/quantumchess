using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {
    public override PieceType typeOfPiece { get; protected set; }
    public override PlayerColor colorOfPiece { get; protected set; }

    protected override GameObject getClearPrefab() {
        return Prefabs.instance.prefQueen;
    }

    public Queen(PlayerColor color) {
        typeOfPiece = PieceType.Queen;
        colorOfPiece = color;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(RookDirections);
        directions.AddRange(BishopDirections);

        foreach (Vector2Int direction in directions) {
            Vector2Int tempGridPoint = gridPoint;

            for (int i = 0; i < 8; i++) {
                tempGridPoint += direction;
                locations.Add(tempGridPoint);
                if (!layer.isFreedGrid(tempGridPoint))
                    break;
            }
        }
        return locations;
    }
}
