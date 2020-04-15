using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
    public override PieceType typeOfPiece { get; protected set; }

    public Pawn(PieceType typeOfPiece) {
        this.typeOfPiece = typeOfPiece;
    }

    public override List<Vector2Int> getMoveLocations(Layer layer, bool isWhite, Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();

        int fowardDirecton = GameManager.instance.currentPlayer.ZAxis;

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
        if (layer.getPieceIDAtGrid(toCorner) != null)
            locations.Add(toCorner);

        toCorner.x -= 2;
        if (layer.getPieceIDAtGrid(toCorner) != null)
            locations.Add(toCorner);

        return locations;
    }
}
