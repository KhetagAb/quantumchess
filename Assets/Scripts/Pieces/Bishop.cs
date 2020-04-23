using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {
    public override PieceType typeOfPiece { get; protected set; }
    public override PlayerColor colorOfPiece { get; protected set; }

    protected override GameObject getClearPrefab() {
        return Prefabs.instance.prefBishop;
    }

    public Bishop(PlayerColor color) {
        typeOfPiece = PieceType.Bishop;
        colorOfPiece = color;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int direction in BishopDirections) {
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
