using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static int limitLayers = 10; //defautl

    [SerializeField] private TextMeshProUGUI currentHarmonicText;
    [SerializeField] private GameObject WaringWindow;
    [SerializeField] private GameObject PromotationWindow;
    [SerializeField] private Text showCurrentPlayer;

    [SerializeField] private PieceSelection PS;
    [SerializeField] private SimpleSelection SS;
    [SerializeField] private QuantumSelection QS;

    private int harmonicToShow = 0;
    private int currentHarmonic { 
        get { return harmonicToShow; }
        set {
            if (0 <= value && value < layers.Count)
                harmonicToShow = value;
        }
    }

    public List<Layer> layers = new List<Layer>();
    public Piece[,] quantumStatePiece = new Piece[8, 8];
    public int[,] quantumStateCount = new int[8, 8];

    private Player white;
    private Player black;
    private int k = 1;
    public Player curPlayer { 
        get { 
            return (k == 0) ? white : black; 
        }  
    }

    private void Awake() {
        instance = this;

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

        afterMove(new Vector2Int(0, 0));
    }
    private void InstallSetPiece(Piece piece, int col, int row) {
        layers[0].pieces[col, row] = piece;
    }

    public void nextStep(Piece[,] board, int[,] quantums) {
        Display.instance.ToNextStep(board, quantums);

        k ^= 1;
        showCurrentPlayer.text = "current player: " + curPlayer.color.ToString().ToLower();

        Debug.Log(layers.Count);
    }

    public void ShowHarmonics() {
        SS.Disactivate();
        QS.Disactivate();
        PS.Disactivate();

        currentHarmonic = 0;
        renderHarmonic();
    }

    public void NextHarmonic() {
        currentHarmonic++;
        renderHarmonic();
    }

    public void PrevHarmonic() {
        currentHarmonic--;
        renderHarmonic();
    }

    private void renderHarmonic() {
        currentHarmonicText.text = "#" + (currentHarmonic + 1).ToString() + " Weight: " + layers[currentHarmonic].weight;
        Display.instance.showTheBoard(layers[currentHarmonic].pieces);
    }

    public void HideHaronics() {
        Display.instance.showTheBoard(quantumStatePiece, quantumStateCount);

        PS.Activate();
    }

    // ============================================[FUNCTIONAL]
    public Piece getPieceAtGrid(Vector2Int gridPoint) {
        return quantumStatePiece[gridPoint.x, gridPoint.y];
    }
    public int sumOfLayersWeight() {
        int sum = 0;
        foreach (Layer layer in layers)
            sum += layer.weight;

        return sum;
    }

    // ============================================[LOCATIONS]
    public List<Vector2Int> getMoveLocations(Vector2Int gridPoint, bool isQuant) {
        Piece piece = getPieceAtGrid(gridPoint);
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        foreach (Layer layer in layers) {
            List<Vector2Int> allowedGridsInLayer = layer.getMoveLocationsInLayerInStep(piece, gridPoint, isQuant);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Commn.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }
    public List<Vector2Int> getMoveLocations(Vector2Int startPoint, Vector2Int midPoint) {
        Piece piece = getPieceAtGrid(startPoint);
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        foreach (Layer layer in layers) {
            List<Vector2Int> allowedGridsInLayer = layer.getMoveLocationsInLayerInMid(piece, startPoint, midPoint);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Commn.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }

    // ============================================[MOVES]
    public bool isQuantumMovePos(Step step) {
        Piece piece = getPieceAtGrid(step.from);

        int sumOfLayersWeight = 0;
        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInMid(piece, step)) {
                sumOfLayersWeight++;
            } else {
                sumOfLayersWeight += 2;
            }
        }

        if (sumOfLayersWeight > (1 << limitLayers)) {
            WaringWindow.SetActive(true);
            return false;
        } else {
            return true;
        }
    }

    public void QuantumMove(Step step) {
        Piece piece = getPieceAtGrid(step.from);
        List<Layer> toAddLayers = new List<Layer>();

        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInMid(piece, step)) {
                toAddLayers.Add(new Layer(layer));
                layer.moveFromTo(step);
            } else {
                layer.weight *= 2;
            }
        }

        layers.AddRange(toAddLayers);

        afterMove(step.to);
    }
    public void SimpleMove(Step step) {
        Piece piece = getPieceAtGrid(step.from);

        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInStep(piece, step, false))
                layer.moveFromTo(step);
        }

        afterMove(step.to);
    }
    public void Castle(int index) {
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

        afterMove(CastleArrow.rookAlphaToGrids[index]);
    }

    // ============================================[AFTER MOVE]

    public void afterMove(Vector2Int finishGridPoint) {
        resolveConflicts(finishGridPoint);
        quantumNormalize();
        deleteDublicates();

        if (!isPawnPromotion(finishGridPoint) == true)
            nextStep(quantumStatePiece, quantumStateCount);
    }

    private Vector2Int toPromote;
    public bool isPawnPromotion(Vector2Int gridPoint) {
        Piece piece = getPieceAtGrid(gridPoint);
        if (piece != null && piece.typeOfPiece == PieceType.Pawn && (gridPoint.y == 0 || gridPoint.y == 7)) {
            toPromote = gridPoint;
            PromotationWindow.SetActive(true);
            return true;
        } else {
            return false;
        }
    }

    public void pawnPromotion(int ind) { // можно поменять
        Piece promote = null;
        switch (ind) {
            case 0:
                promote = new Queen(curPlayer.color);
            break;
            case 1:
                promote = new Bishop(curPlayer.color);
            break;
            case 2:
                promote = new Knight(curPlayer.color);
            break;
            case 3:
                promote = new Rook(curPlayer.color);
            break;
        }

        int col = toPromote.x, row = toPromote.y;
        foreach (Layer layer in layers) {
            if (layer.pieces[col, row] != null)
                layer.pieces[col, row] = promote;
        }

        quantumNormalize();

        nextStep(quantumStatePiece, quantumStateCount);
    }

    private void resolveConflicts(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;

        Dictionary<Piece, int> status = new Dictionary<Piece, int>();
        foreach (Layer layer in layers) {
            Piece curID = layer.pieces[col, row];
            if (curID != null) {
                if (status.ContainsKey(curID))
                    status[curID] += layer.weight;
                else
                    status.Add(curID, layer.weight);
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
            Debug.LogAssertionFormat("All: {0}", sum);
            Debug.LogAssertionFormat("ID: {0} Count: {1}. Capc: {2}", firstID.Key, firstID.Value, first);
            Debug.LogAssertionFormat("ID: {0} Count: {1}. Capc: {2}", secondID.Key, secondID.Value, sum - first);

            if (first <= firstID.Value) {
                Debug.LogAssertionFormat("{0} WIN!", firstID.Key);
                layers.RemoveAll(ID => ID.pieces[col, row] == secondID.Key);
            } else {
                Debug.LogAssertionFormat("{0} WIN!", secondID.Key);
                layers.RemoveAll(ID => ID.pieces[col, row] == firstID.Key);
            }
        }
    }
    public void quantumNormalize() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                Piece piece = null;
                int count = 0;

                foreach (Layer layer in layers) {
                    if (layer.pieces[i, j] != null) {
                        piece = layer.pieces[i, j];
                        count += layer.weight;
                    }
                }

                quantumStatePiece[i, j] = piece;
                quantumStateCount[i, j] = count;
            }
        }
    }

    System.Random rnd = new System.Random();
    private void deleteDublicates() {
        layers.Sort((l1, l2) => string.Compare(l1.getDate(), l2.getDate()));

        List<Layer> newLayers = new List<Layer>();

        int weight = layers[0].weight;
        for (int i = 1; i < layers.Count; i++) {
            if (layers[i].getDate() == layers[i - 1].getDate()) {
                weight += layers[i].weight;
            } else {
                newLayers.Add(layers[i - 1]);
                newLayers[newLayers.Count - 1].weight = weight;
                weight = layers[i].weight;
            }
        }
        newLayers.Add(layers[layers.Count - 1]);
        newLayers[newLayers.Count - 1].weight = weight;

        layers = newLayers;

     //   layers.Sort((l1, l2) => (rnd.Next(100) < 50 ? 1 : -1));
    }
}
