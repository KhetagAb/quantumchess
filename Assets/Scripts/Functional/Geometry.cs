using UnityEngine;

public class Geometry {
    static public Vector3 PointFromGrid(Vector2Int gridPoint) {
        float x = gridPoint.x;
        float z = gridPoint.y;
        return new Vector3(x, 0, z);
    }

    static public Vector2Int GridFromPoint(Vector3 point) {
        int col = Mathf.FloorToInt(point.x + 0.5f);
        int row = Mathf.FloorToInt(point.z + 0.5f);
        return new Vector2Int(col, row);
    }

    static public Vector3 PointFrom(int col, int row) {
        return new Vector3(col, 0, row);
    }

    static public Vector2Int GridFrom(int col, int row) {
        return new Vector2Int(col, row);
    }
}
