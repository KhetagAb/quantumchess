using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
    public override PieceType typeOfPiece { get; protected set; }
    public override PlayerColor colorOfPiece { get; protected set; }

    protected override GameObject getClearPrefab() {
        return Prefabs.instance.prefPawn;
    }

    public Pawn(PlayerColor color) {
        typeOfPiece = PieceType.Pawn;
        colorOfPiece = color;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, Vector2Int gridPoint) {
        bool isWhite = (colorOfPiece == PlayerColor.White);
        List<Vector2Int> locations = new List<Vector2Int>();

        int fowardDirecton = GameManager.instance.curPlayer.ZAxis;

        Vector2Int toFoward = new Vector2Int(gridPoint.x, gridPoint.y + fowardDirecton);
        if (layer.isAllowedGrid(toFoward)) {
            locations.Add(toFoward);
            toFoward.y += fowardDirecton;
            if (layer.isAllowedGrid(toFoward)) {
                if (isWhite && gridPoint.y == 1)
                    locations.Add(toFoward);
                else if (!isWhite && gridPoint.y == 6) 
                    locations.Add(toFoward);
            }
            toFoward.y -= fowardDirecton;
        }

        Vector2Int toCorner = new Vector2Int(toFoward.x + 1, toFoward.y);
        if (layer.getPieceAtGrid(toCorner) != null)
            locations.Add(toCorner);

        toCorner.x -= 2;
        if (layer.getPieceAtGrid(toCorner) != null)
            locations.Add(toCorner);

        return locations;
    }
}
