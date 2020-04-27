using UnityEngine;

public class StepAndPieceSelection : MonoBehaviour {
    protected bool isCorrectHit(RaycastHit hitPlace) {
        Vector2Int grid = getGridFromHit(hitPlace);
        return (0 <= grid.x && grid.x <= 7 && 0 <= grid.y && grid.y <= 7);
    }
    protected Vector2Int getGridFromHit(RaycastHit hitplace) {
        return Geometry.GridFromPoint(hitplace.point);
    }
    protected Piece getPieceAtGrid(Vector2Int gridPoint) {
        return Step.instance.getPieceAtGrid(gridPoint);
    }
    protected bool isFriendlyPieceAtGrid(Vector2Int gridPoint) {
        Piece piece = getPieceAtGrid(gridPoint);
        if (piece == null)
            return false;

        return GameManager.instance.curPlayer.color == piece.colorOfPiece;
    }
}
