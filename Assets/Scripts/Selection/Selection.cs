using UnityEngine;

public class Selection : MonoBehaviour {
    protected GameObject selectTile;

    protected void showObjOnGrid(GameObject obj, Vector2Int gridPoint) {
        obj.transform.position = Geometry.PointFromGrid(gridPoint);
        obj.SetActive(true);
    }
    protected void hideObj(GameObject obj) {
        obj.SetActive(false);
    }

    protected bool isCorrectHit(RaycastHit hitPlace) {
        Vector2Int grid = getGridFromHit(hitPlace);
        return (0 <= grid.x && grid.x <= 7 && 0 <= grid.y && grid.y <= 7);
    }
    protected Vector2Int getGridFromHit(RaycastHit hitplace) {
        return Geometry.GridFromPoint(hitplace.point);
    }

    protected Piece getPieceAtGrid(Vector2Int gridPoint) {
        return GameManager.instance.getPieceAtGrid(gridPoint);
    }
    protected bool isFriendlyPieceAtGrid(Vector2Int gridPoint) {
        Piece piece = getPieceAtGrid(gridPoint);
        if (piece == null)
            return false;

        return GameManager.instance.curPlayer.color == piece.colorOfPiece;
    }
}
