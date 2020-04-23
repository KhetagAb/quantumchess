using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CastleArrow : MonoBehaviour {
    [SerializeField] protected int index;
    public static bool[] isDeny = new bool[4] { true, true, true, true };

    private void Start() {
        hideCastle();
    }

    public void setDenyStatus(bool status) {
        isDeny[index] = status;

        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        if (!isDeny[index])
            mesh.material = Prefabs.instance.defaultCastle;
        else
            mesh.material = Prefabs.instance.denyCastle;
    }

    // Хранилище префабов по ID'шникам
    protected static Piece[] piecesKingAlpha = new Piece[4]; // whiteShort, whiteLong, blackShort, blackLong
    protected static Piece[] piecesRookAlpha = new Piece[4]; // whiteShort, whiteLong, blackShort, blackLong

    // Массивы координат установки
    public static List<Vector2Int> kingAlphaToGrids = new List<Vector2Int>() {
        new Vector2Int(6, 0), new Vector2Int(2, 0), new Vector2Int(6, 7), new Vector2Int(2, 7) };
    public static List<Vector2Int> rookAlphaToGrids = new List<Vector2Int>() {
        new Vector2Int(5, 0), new Vector2Int(3, 0), new Vector2Int(5, 7), new Vector2Int(3, 7) };

    private void Awake() {
        piecesKingAlpha[index] = new King(index <= 1 ? PlayerColor.White : PlayerColor.Black);
        piecesRookAlpha[index] = new Rook(index <= 1 ? PlayerColor.White : PlayerColor.Black);
    }

    public void showCastle() {
        gameObject.SetActive(true);
    }
    public void hideCastle() {
        hideAlphaAndTile();
        gameObject.SetActive(false);
    }

    protected void showAlphaAndTile() {
        Display.instance.addPieceAtGrid(piecesKingAlpha[index], kingAlphaToGrids[index], true, false);
        Display.instance.addPieceAtGrid(piecesRookAlpha[index], rookAlphaToGrids[index], true, false);
        Display.instance.setPermTileAtGrid(kingAlphaToGrids[index]);
    }
    protected void hideAlphaAndTile() {
        Display.instance.delPermTileAtGrid(kingAlphaToGrids[index]);
        Display.instance.delNotPermPieceAtGrid(kingAlphaToGrids[index]);
        Display.instance.delNotPermPieceAtGrid(rookAlphaToGrids[index]);
    }
}
