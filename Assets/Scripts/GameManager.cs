using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [SerializeField] public GameObject BoardObjectOnScene;
    [SerializeField] private Display display;

    private int k = 1;
    private Player white;
    private Player black;
    public Player curPlayer { get { return (k == 0) ? white : black; }  }

    public void nextPlayer() {
        k ^= 1;
    }

    public static List<Layer> layers;
    public static KeyValuePair<Piece, int>[,] quantumState;

    private void Awake() {
        instance = this;

        quantumState = new KeyValuePair<Piece, int>[8, 8];
        layers = new List<Layer>();

        white = new Player(PlayerColor.White);
        black = new Player(PlayerColor.Black);
    }

    private void Start() {
        Installation();
    }

    private void Installation() {
        layers.Add(new Layer());

        InstallSetPiece(new Rook(PlayerColor.White), 0, 0);
        InstallSetPiece(new Knight(PlayerColor.White), 1, 0);
        InstallSetPiece(new Bishop(PlayerColor.White), 2, 0);
        InstallSetPiece(new Queen(PlayerColor.White), 3, 0);
        InstallSetPiece(new King(PlayerColor.White), 4, 0);
        InstallSetPiece(new Bishop(PlayerColor.White), 5, 0);
        InstallSetPiece(new Knight(PlayerColor.White), 6, 0);
        InstallSetPiece(new Rook(PlayerColor.White), 7, 0);
        for (int i = 0; i < 8; i++)
            InstallSetPiece(new Pawn(PlayerColor.White), i, 1);

        InstallSetPiece(new Rook(PlayerColor.Black), 0, 7);
        InstallSetPiece(new Knight(PlayerColor.Black), 1, 7);
        InstallSetPiece(new Bishop(PlayerColor.Black), 2, 7);
        InstallSetPiece(new Queen(PlayerColor.Black), 3, 7);
        InstallSetPiece(new King(PlayerColor.Black), 4, 7);
        InstallSetPiece(new Bishop(PlayerColor.Black), 5, 7);
        InstallSetPiece(new Knight(PlayerColor.Black), 6, 7);
        InstallSetPiece(new Rook(PlayerColor.Black), 7, 7);
        for (int i = 0; i < 8; i++)
            InstallSetPiece(new Pawn(PlayerColor.Black), i, 6);

        quantumNormalize();

        display.Activate();
    }
    private void InstallSetPiece(Piece piece, int col, int row) {
        layers[0].pieces[col, row] = piece;
    }

    public List<Vector2Int> getAllowedGridsInStep(Vector2Int gridPoint, bool isQunt) {
        return getMoveLocations(gridPoint, isQunt);
    }
    public List<Vector2Int> getAllowedGridsInMidStep(Vector2Int startGridPoint, Vector2Int midGridPoint) {
        return getMoveLocations(startGridPoint, midGridPoint);
    }

    // ===================================================[STEPS] 
    public void simpleMove(Vector2Int startGridPoint, Vector2Int finishGridPoint) {
        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInStep(startGridPoint, finishGridPoint, false)) {
                layer.moveFromTo(startGridPoint, finishGridPoint);
            }
        }

        AfterMove(finishGridPoint);
    }
    public void quantumMove(Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        List<Layer> toAddLayers = new List<Layer>();
        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInMid(startGridPoint, midGridPoint, finishGridPoint)) {
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

        Dictionary<Piece, int> status = new Dictionary<Piece, int>();
        for (int i = 0; i < layers.Count; i++) {
            Piece curID = layers[i].pieces[col, row];
            if (curID != null) {
                if (status.ContainsKey(curID))
                    status[curID] += layers[i].weight;
                else
                    status.Add(curID, layers[i].weight);
            }
        }

        if (status.Count > 1) {
            KeyValuePair<Piece, int> firstID;
            KeyValuePair<Piece, int> secondID;

            foreach (KeyValuePair<Piece, int> cur in status) {
                if (firstID.Key == null)
                    firstID = new KeyValuePair<Piece, int>(cur.Key, cur.Value);
                else
                    secondID = new KeyValuePair<Piece, int>(cur.Key, cur.Value);
            }

            float sum = firstID.Value + secondID.Value;
            float first = UnityEngine.Random.Range(0.0f, sum); // TrulyRandom 

            // DEBUG
            Debug.LogAssertion("All: " + sum);
            Debug.LogAssertion("ID: " + firstID.Key + ", Count: " + firstID.Value + ". Capc: " + first);
            Debug.LogAssertion("ID: " + secondID.Key + " , Count: " + secondID.Value + ". Capc: " + (sum - first));

            if (first <= firstID.Value) {
                Debug.LogAssertion(firstID.Key + " WIN!");
                layers.RemoveAll(ID => ID.pieces[col, row] == secondID.Key);
            } else {
                Debug.LogAssertion(secondID.Key + " WIN!");
                layers.RemoveAll(ID => ID.pieces[col, row] == firstID.Key);
            }
        }
    }

    // ===================================================[LAYERS]
    private List<Vector2Int> getMoveLocations(Vector2Int gridPoint, bool isQuant) {
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        for (int i = 0; i < layers.Count; i++) {
            List<Vector2Int> allowedGridsInLayer = layers[i].getMoveLocationsInLayerInStep(gridPoint, isQuant);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Piece.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }
    private List<Vector2Int> getMoveLocations(Vector2Int startPoint, Vector2Int midPoint) {
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        for (int i = 0; i < layers.Count; i++) {
            List<Vector2Int> allowedGridsInLayer = layers[i].getMoveLocationsInLayerInMid(startPoint, midPoint);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Piece.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }

    // ===================================================[QUANTUM]
    public Piece getPieceAtGrid(int col, int row) {
        return quantumState[col, row].Key;
    }
    public Piece getPieceAtGrid(Vector2Int? gridPoint) {
        if (gridPoint == null)
            return null;

        return getPieceAtGrid(((Vector2Int) gridPoint).x, ((Vector2Int) gridPoint).y);
    }
    public PieceType? getPieceTypeByGrid(Vector2Int grid) {
        Piece piece = getPieceAtGrid(grid);

        if (piece == null)
            return null;

        return piece.typeOfPiece;
    }
    private void quantumNormalize() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                Piece piece = null;
                int count = 0;

                for (int k = 0; k < layers.Count; k++) {
                    if (layers[k].pieces[i, j] != null) {
                        piece = layers[k].pieces[i, j];
                        count += layers[k].weight;
                    }
                }

                quantumState[i, j] = new KeyValuePair<Piece, int>(piece, count);
            }
        }
    }

    // ===================================================[DISPLAY]
    public void selectSimplePieceAtGrid(Vector2Int gridPoint) {
        display.selectSimplePieceAtGrid(gridPoint);
    }
    public void selectQuantumPieceAtGrid(Vector2Int gridPoint) {
        display.selectQuantumPieceAtGrid(gridPoint);
    }
    public void deselectPieceAtGrid(Vector2Int gridPoint) {
        display.deselectPieceAtGrid(gridPoint);
    }
}
