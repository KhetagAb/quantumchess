using System.Collections.Generic;
using UnityEngine;

public enum PieceType {King, Queen, Bishop, Knight, Rook, Pawn};

public abstract class Piece {
    public abstract PieceType typeOfPiece { get; protected set; }
    public abstract PlayerColor colorOfPiece { get; protected set; }
    public abstract List<Vector2Int> getMoveLocations(Layer layer, Vector2Int gridPoint);
    protected abstract GameObject getClearPrefab();

    public GameObject getAlphaPrefab() {
        GameObject temp = getClearPrefab();
        Material toSet = getAlphaMaterial();

        foreach (MeshRenderer mesh in temp.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;

        return temp;
    }
    public GameObject getPrefab() {
        GameObject temp = getClearPrefab();
        Material toSet = getDefaultMaterial();

        foreach (MeshRenderer mesh in temp.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;

        return temp;
    }

    protected static Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected static Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};
    protected static Vector2Int[] KnightDirections = {new Vector2Int(1,2), new Vector2Int(2, 1),
        new Vector2Int(1, -2), new Vector2Int(-2, 1),
        new Vector2Int(2, -1), new Vector2Int(-1, 2),
        new Vector2Int(-2, -1), new Vector2Int(-1, -2)};

    public static void AddLocation(Vector2Int gridPoint, List<Vector2Int> locations) {
        if (!locations.Contains(gridPoint))
            locations.Add(gridPoint);
    }

    public Material getAlphaMaterial() {
        if (colorOfPiece == PlayerColor.White)
            return Prefabs.instance.defaultWhiteAlpha;
        else
            return Prefabs.instance.defaultBlackAlpha;
    }
    public Material getDefaultMaterial() {
        if (colorOfPiece == PlayerColor.White)
            return Prefabs.instance.defaultWhite;
        else
            return Prefabs.instance.defaultBlack;
    }
    public Material getSimpleSelection() {
        if (colorOfPiece == PlayerColor.White)
            return Prefabs.instance.simpleWhiteSel;
        else
            return Prefabs.instance.simpleBlackSel;
    }
    public Material getQuantumSelection() {
        if (colorOfPiece == PlayerColor.White)
            return Prefabs.instance.quantumWhiteSel;
        else
            return Prefabs.instance.quantumBlackSel;
    }
}
