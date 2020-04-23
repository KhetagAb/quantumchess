using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Display : MonoBehaviour {
    public static Display instance;

    [SerializeField] private Text showCurrentPlayer;

    public Vector2Int? selector = null;
    public PieceAtGrid[,] board;

    private void Awake() {
        instance = this;
        this.enabled = false;

        board = new PieceAtGrid[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                GameObject temp = Instantiate(Prefabs.instance.piece, Geometry.PointFrom(i, j), Prefabs.instance.piece.transform.rotation, transform);
                board[i, j] = temp.GetComponent<PieceAtGrid>();
            }
        }
    }
    private void FixedUpdate() {
        // ? плавное удаленеи фигур
        Disactivate();
    }

    private void updateTheBoard() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                Vector2Int gridPoint = new Vector2Int(i, j);
                Piece piece = GameManager.instance.getPieceAtGrid(gridPoint);
                int? qdeb = quantumDebug(i, j);

                setPieceAtGrid(piece, gridPoint);
                setQDAtGrid(qdeb, gridPoint);
            }
        }

        GameManager.instance.nextPlayer();
        showCurrentPlayer.text = "Current player: " + GameManager.instance.curPlayer.color.ToString();
    }

    public void Activate() {
        this.enabled = true;
    }
    public void Activate(Vector2Int startGridPoint, Vector2Int finishGridPoint, bool isQuant) {
        // просчёт порядка действий ?

        Activate();
    }
    private void Disactivate() {
        this.enabled = false;

        updateTheBoard();

        PieceSelection goTo = GetComponent<PieceSelection>();
        goTo.Activate();
    }

    private int? quantumDebug(int col, int row) {
        int sum = 0;
        for (int i = 0; i < GameManager.layers.Count; i++)
            sum += GameManager.layers[i].weight;

        if (GameManager.quantumState[col, row].Value == 0)
            return null;
        else
            return (int) (((float) GameManager.quantumState[col, row].Value / sum) * 100);
    }

    public void setSelectorAtGrid(Vector2Int? gridPoint, Piece piece = null) {
        if (selector == gridPoint)
            return;
        
        if (gridPoint != null) {
            if (selector != null) {
                delSelTileAtGrid((Vector2Int) selector);
            }
        
            selector = gridPoint;
            setSelTileAtGrid((Vector2Int) selector, piece);
        } else {
            delSelTileAtGrid((Vector2Int) selector);
            selector = null;
        }
    }
    public void setPermTiles(List<Vector2Int> grids) {
        foreach (Vector2Int grid in grids) {
            setPermTileAtGrid(grid);
        }
    }
    public void delPermTiles(List<Vector2Int> grids) {
        foreach (Vector2Int grid in grids) {
            delPermTileAtGrid(grid);
            delNotPermPieceAtGrid(grid);
        }
    }

    public void addPieceAtGrid(Piece piece, Vector2Int gridPoint, bool isAlpha = false, bool isPerm = true) {
        board[gridPoint.x, gridPoint.y].addPiece(piece, isAlpha, isPerm);
    }
    public void setPieceAtGrid(Piece piece, Vector2Int gridPoint, bool isAlpha = false, bool isPerm = true) {
        board[gridPoint.x, gridPoint.y].setPiece(piece, isAlpha, isPerm);
    }
    public void setPermPieceAtGrid(Piece piece, Vector2Int gridPoint, bool isAlpha = false, bool isPerm = true) {
        board[gridPoint.x, gridPoint.y].setPermPiece(piece, isAlpha, isPerm);
    }
    public void delPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delPiece();
    }
    public void delAlphaPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delAlphaPiece();
    }

    public void delNotPermPieceAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delNotPermPiece();
    }

    public void setPermTileAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].setPermTile();
        board[gridPoint.x, gridPoint.y].updatePermTile();
    }
    public void delPermTileAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delPermTile();
    }

    private void setSelTileAtGrid(Vector2Int gridPoint, Piece piece = null) {
        board[gridPoint.x, gridPoint.y].setSelTile(piece);
    }
    private void delSelTileAtGrid(Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].delSelTile();
    }

    private void setQDAtGrid(int? qd, Vector2Int gridPoint) {
        board[gridPoint.x, gridPoint.y].setQD(qd);
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
