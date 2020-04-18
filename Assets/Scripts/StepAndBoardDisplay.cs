using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StepAndBoardDisplay : MonoBehaviour {
    [SerializeField] private Text showCurrentPlayer;

    public GameObject[,] storageObjects;
    private int?[,] IDs;

    private void Awake() {
        this.enabled = false;

        storageObjects = new GameObject[8, 8];
        IDs = new int?[8, 8];
    }

    private void FixedUpdate() {
        // ? плавное удаленеи фигур
        Disactivate();
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

    private void updateTheBoard() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                int? IDAtGrid = GameManager.instance.getPieceIDAtGrid(i, j);

                if (IDAtGrid == null) {
                    if (IDs[i, j] != null)
                        DeletePieceAtGrid(i, j);
                } else {
                    if (IDs[i, j] != IDAtGrid) {
                        DeletePieceAtGrid(i, j);
                        AddPieceAtGrid((int) IDAtGrid, i, j);
                    }

                    storageObjects[i, j].GetComponentInChildren<TextMeshPro>().text = quantumDebug(i, j).ToString() + "%";
                }
            }
        }

        GameManager.instance.nextPlayer();
        showCurrentPlayer.text = "Current player: " + GameManager.instance.currentPlayer.name.ToString();
    }

    private int quantumDebug(int col, int row) {
        int sum = 0;
        for (int i = 0; i < GameManager.layers.Count; i++)
            sum += GameManager.layers[i].weight;

        return (int) (((float) GameManager.quantumState[col, row].y / sum) * 100);
    }

    // =========================[ADD/DEL]========================
    private void AddPieceAtGrid(int ID, int col, int row) {
        GameObject tempPrefab = PrefabIndexing.getPrefabByID(ID);
        storageObjects[col, row] = Instantiate(tempPrefab, Geometry.PointFromGrid(new Vector2Int(col, row)), tempPrefab.transform.rotation, gameObject.transform);
        IDs[col, row] = ID;
    }

    private void DeletePieceAtGrid(int col, int row) {
        Destroy(storageObjects[col, row]);
        storageObjects[col, row] = null;
        IDs[col, row] = null;
    }

    // =========================[STEPS]========================= 
    public void selectSimplePieceAtGrid(Vector2Int gridPoint) {
        MeshRenderer mesh = storageObjects[gridPoint.x, gridPoint.y].GetComponentInChildren<MeshRenderer>();
        mesh.material = PrefabIndexing.instance.getCorrectSelectMaterial(GameManager.instance.currentPlayer.name, false);
    }

    public void selectQuantumPieceAtGrid(Vector2Int gridPoint) {
        MeshRenderer mesh = storageObjects[gridPoint.x, gridPoint.y].GetComponentInChildren<MeshRenderer>();
        mesh.material = PrefabIndexing.instance.getCorrectSelectMaterial(GameManager.instance.currentPlayer.name, true);
    }

    public void deselectPieceAtGrid(Vector2Int gridPoint) {
        MeshRenderer mesh = storageObjects[gridPoint.x, gridPoint.y].GetComponentInChildren<MeshRenderer>();
        mesh.material = PrefabIndexing.instance.defaultMaterial;
    }
}
