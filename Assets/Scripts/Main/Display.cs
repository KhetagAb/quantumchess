using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Display : MonoBehaviour {
    public static Display instance;
    [SerializeField] public Camera cameraDebug;

    public Vector2Int? selector = null;
    public PieceAtGrid[,] board;

    private void Awake() {
        instance = this;

        board = new PieceAtGrid[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                GameObject temp = Instantiate(Prefabs.instance.piece, Geometry.PointFrom(i, j), Prefabs.instance.piece.transform.rotation, transform);
                board[i, j] = temp.GetComponent<PieceAtGrid>();
            }
        }
    }

    private void FixedUpdate() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++)
                board[i, j].lookat(cameraDebug.transform.position);
        }
    }

    public void ToNextStep(Piece[,] board, int[,] quantums) {
        showTheBoard(board, quantums);

        PieceSelection goTo = GetComponent<PieceSelection>();
        goTo.Activate();
    }

    public void showTheBoard(Piece[,] board, int[,] quantums = null) {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                setPieceAtGrid(board[i, j], new Vector2Int(i, j), false, PieceTypeOnGrid.Permanent);
            }
        }

        quntumDebug(quantums);
    }

    // ======================================================
    private void quntumDebug(int[,] quantums) {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                board[i, j].setQD(quantumDebug(i, j, quantums));
            }
        }
    }
    private int? quantumDebug(int col, int row, int[,] quantums) {
        if (quantums == null)
            return null;

        int sum = GameManager.instance.sumOfLayersWeight();
        if (quantums[col, row] == 0)
            return null;
        else
            return (int) (((float) quantums[col, row] / sum) * 100);
    }
    // ======================================================

    public void delAllNotPermPieces() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                delNotPermPieceAtGrid(new Vector2Int(i, j));
            }
        }
    }
    public void changeTypeToUntilPiece(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].changeTypeToUntilPiece();
    }

    public void addPieceAtGrid(Piece piece, Vector2Int gridPoint, bool isAlpha, PieceTypeOnGrid type) {
        board[gridPoint.x, gridPoint.y].addPiece(piece, isAlpha, type);
    }
    public void setPieceAtGrid(Piece piece, Vector2Int gridPoint, bool isAlpha, PieceTypeOnGrid type) {
        board[gridPoint.x, gridPoint.y].setPiece(piece, isAlpha, type);
    }
    public void delNotPermPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delNotPermPiece();
    }
    public void delPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delPiece();
    }

    public void setTiles(List<Vector2Int> tileGrids) {
        foreach (Vector2Int grid in tileGrids)
            setTileAtGrid(grid);
    }
    public void delTiles(List<Vector2Int> tileGrids) {
        foreach (Vector2Int grid in tileGrids)
            delTileAtGrid(grid);
    }
    public void setTileAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].setTile();
    }
    public void delTileAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delTile();
    }

    public void setSelector(Vector2Int? gridPoint, Piece piece = null, bool checkDiff = true) {
        if (selector == gridPoint && checkDiff) // не нравится. можно оптимизировать.
            return;

        if (gridPoint != null) {
            if (selector != null) {
                delSelectorAtGrid((Vector2Int) selector);
            }

            selector = gridPoint;
            setSelectorAtGrid((Vector2Int) selector, piece);
        } else {
            delSelectorAtGrid((Vector2Int) selector);
            selector = null;
        }
    }
    private void setSelectorAtGrid(Vector2Int gridPoint, Piece piece = null) {
        board[gridPoint.x, gridPoint.y].setSelector(piece);
    }
    private void delSelectorAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delSelector();
    }

    public void selectSimplePieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].selectSimplePieceAtGrid();
    }
    public void selectQuantumPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].selectQuantumPieceAtGrid();
    }
    public void deselectPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].deselectPieceAtGrid();
    }
}
