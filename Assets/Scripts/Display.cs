using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Display : MonoBehaviour {
    [SerializeField] private Text showCurrentPlayer;

    public PieceAtGrid[,] board;

    private void Awake() {
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
                Piece piece = GameManager.instance.getPieceAtGrid(i, j);
                int? qdeb = quantumDebug(i, j);

                board[i, j].updatePiece(piece);
                board[i, j].updateDebugText(qdeb);
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
