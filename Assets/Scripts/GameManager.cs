using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [SerializeField] public GameObject BoardObjectOnScene;
    [SerializeField] private StepAndBoardDisplay Display;

    public Player currentPlayer;
    public Player anotherPlayer;

    public Player white;
    public Player black;

    public static List<Layer> layers;
    public static Vector2Int[,] quantumState;

    private void Awake() {
        instance = this;

        quantumState = new Vector2Int[8, 8];
        layers = new List<Layer>();

        white = new Player(PlayerType.White, true);
        black = new Player(PlayerType.Black, false);

        currentPlayer = black;
        anotherPlayer = white;
    }

    private void Start() {
        for (int i = 0; i <= 5; i++)
            white.playersPieces.Add(i);

        for (int i = 6; i <= 11; i++)
            black.playersPieces.Add(i);

        Installation();
    }

    private void Installation() {
        layers.Add(new Layer());

        InstallSetPiece(4, 0, 0);
        InstallSetPiece(2, 1, 0);
        InstallSetPiece(3, 2, 0);
        InstallSetPiece(1, 3, 0);
        InstallSetPiece(0, 4, 0);
        InstallSetPiece(3, 5, 0);
        InstallSetPiece(2, 6, 0);
        InstallSetPiece(4, 7, 0);
        for (int i = 0; i < 8; i++)
            InstallSetPiece(5, i, 1);

        InstallSetPiece(10, 0, 7);
        InstallSetPiece(8, 1, 7);
        InstallSetPiece(9, 2, 7);
        InstallSetPiece(6, 4, 7);
        InstallSetPiece(7, 3, 7);
        InstallSetPiece(9, 5, 7);
        InstallSetPiece(8, 6, 7);
        InstallSetPiece(10, 7, 7);
        for (int i = 0; i < 8; i++)
            InstallSetPiece(11, i, 6);

        quantumNormalize();

        Display.Activate();
    }
    private void InstallSetPiece(int ID, int col, int row) {
        layers[0].pieces[col, row] = ID;
    }

    public List<Vector2Int> getAllowedGridsInStep(Vector2Int gridPoint, bool isQunt) {
        return getAllowedGridsInStep((int) getPieceIDAtGrid(gridPoint), gridPoint, isQunt);
    }
    public List<Vector2Int> getAllowedGridsInStep(int ID, Vector2Int gridPoint, bool isQunt) {
        return getMoveLocations(ID, gridPoint, isQunt);
    }
    public List<Vector2Int> getAllowedGridsInMidStep(Vector2Int startGridPoint, Vector2Int midGridPoint) {
        return getMoveLocations((int) getPieceIDAtGrid(startGridPoint), startGridPoint, midGridPoint);
    }

    public void nextPlayer() {
        Player temp = currentPlayer;
        currentPlayer = anotherPlayer;
        anotherPlayer = temp;
    }

    // ===================================================[STEPS] 
    public void simpleMove(Vector2Int startGridPoint, Vector2Int finishGridPoint) {
        int ID = (int) getPieceIDAtGrid(startGridPoint);

        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInStep(ID, startGridPoint, finishGridPoint, false)) {
                layer.moveFromTo(startGridPoint, finishGridPoint);
            }
        }

        AfterMove(finishGridPoint);
    }
    public void quantumMove(Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        int ID = (int) getPieceIDAtGrid(startGridPoint);

        List<Layer> toAddLayers = new List<Layer>();
        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInMid(ID, startGridPoint, midGridPoint, finishGridPoint)) {
                toAddLayers.Add(new Layer(layer));
                layer.moveFromTo(startGridPoint, finishGridPoint);
            } else {
                layer.weight *= 2;
            }
        }

        layers.AddRange(toAddLayers);

        AfterMove(finishGridPoint);
    }
    public void castle(int index) {
        foreach (Layer layer in layers) {
            if (layer.isCastleLegal(index)) {
                layer.castleKing(index);
            }
        }

        resolveConflicts(CastleArrow.kingAlphaToGrids[index]);

        foreach (Layer layer in layers) {
            if (layer.isCastleLegal(index)) {
                layer.castleRook(index);
            }
        }

        AfterMove(CastleArrow.rookAlphaToGrids[index]);
    } 
   
    public void AfterMove(Vector2Int lastFinishGridPoint) {
        resolveConflicts(lastFinishGridPoint);
        quantumNormalize();
    }
    private void resolveConflicts(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;

        Dictionary<int, int> status = new Dictionary<int, int>();
        for (int i = 0; i < layers.Count; i++) {
            int? curID = layers[i].pieces[col, row];
            if (curID != null) {
                if (status.ContainsKey((int) curID))
                    status[(int) layers[i].pieces[col, row]] += layers[i].weight;
                else
                    status.Add((int) curID, layers[i].weight);
            }
        }

        if (status.Count > 1) {
            Vector2Int firstID = new Vector2Int(-1, -1);
            Vector2Int secondID = new Vector2Int(-1, -1);

            foreach (KeyValuePair<int, int> cur in status) {
                if (firstID.x == -1)
                    firstID = new Vector2Int(cur.Key, cur.Value);
                else
                    secondID = new Vector2Int(cur.Key, cur.Value);
            }

            float sum = firstID.y + secondID.y;
            float first = Random.Range(0.0f, sum); // TrulyRandom 

            // DEBUG
            Debug.LogAssertion("All: " + sum);
            Debug.LogAssertion("ID: " + firstID.x + ", Count: " + firstID.y + ". Capc: " + first);
            Debug.LogAssertion("ID: " + secondID.x + " , Count: " + secondID.y + ". Capc: " + (sum - first));

            if (first <= firstID.y) {
                Debug.LogAssertion(firstID.x + " WIN!");
                layers.RemoveAll(ID => ID.pieces[col, row] == secondID.x);
            } else {
                Debug.LogAssertion(secondID.x + " WIN!");
                layers.RemoveAll(ID => ID.pieces[col, row] == firstID.x);
            }
        }
    }

    // ===================================================[LAYERS]
    private List<Vector2Int> getMoveLocations(int ID, Vector2Int gridPoint, bool isQuant) {
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        for (int i = 0; i < layers.Count; i++) {
            List<Vector2Int> allowedGridsInLayer = layers[i].getMoveLocationsInLayerInStep(ID, gridPoint, isQuant);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Piece.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }
    private List<Vector2Int> getMoveLocations(int ID, Vector2Int startPoint, Vector2Int midPoint) {
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        for (int i = 0; i < layers.Count; i++) {
            List<Vector2Int> allowedGridsInLayer = layers[i].getMoveLocationsInLayerInMid(ID, startPoint, midPoint);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Piece.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }

    // ===================================================[QUANTUM]
    public int? getPieceIDAtGrid(int col, int row) {
        if (quantumState[col, row].x == -1)
            return null;

        return quantumState[col, row].x;
    }
    public int? getPieceIDAtGrid(Vector2Int? gridPoint) {
        if (gridPoint == null)
            return null;

        return getPieceIDAtGrid(((Vector2Int) gridPoint).x, ((Vector2Int) gridPoint).y);
    }
    public PieceType? getPieceTypeByGrid(Vector2Int grid) {
        if (getPieceIDAtGrid(grid) == null)
            return null;

        return getPieceTypeByID((int) getPieceIDAtGrid(grid));
    }
    private void quantumNormalize() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                int? ID = null;
                int count = 0;

                for (int k = 0; k < layers.Count; k++) {
                    if (layers[k].pieces[i, j] != null) {
                        ID = (int) layers[k].pieces[i, j];
                        count += layers[k].weight;
                    }
                }

                if (ID == null)
                    quantumState[i, j] = new Vector2Int(-1, -1);
                else
                    quantumState[i, j] = new Vector2Int((int) ID, count);
            }
        }
    }

    // ===================================================[STEP]
    public void selectSimplePieceAtGrid(Vector2Int gridPoint) {
        Display.selectSimplePieceAtGrid(gridPoint);
    }
    public void selectQuantumPieceAtGrid(Vector2Int gridPoint) {
        Display.selectQuantumPieceAtGrid(gridPoint);
    }
    public void deselectPieceAtGrid(Vector2Int gridPoint) {
        Display.deselectPieceAtGrid(gridPoint);
    }

    // ===================================================[FUNCTIONAL]
    public PieceType? getPieceTypeByID(int ID) {
        return PrefabIndexing.getPieceTypeByID(ID);
    }
}
