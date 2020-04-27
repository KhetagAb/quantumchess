using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour {
    public static int limitLayers;
    public static Step instance;

    private Step() {
        if (instance != null)
            Debug.LogAssertionFormat("Dublicate");
        instance = this;
    }

    public List<Layer> layers = new List<Layer>();
    public Piece[,] quantumStatePiece = new Piece[8, 8];
    public int[,] quantumStateCount = new int[8, 8];

    public Piece getPieceAtGrid(Vector2Int gridPoint) {
        return quantumStatePiece[gridPoint.x, gridPoint.y];
    }
    public int sumOfLayersWeight() {
        int sum = 0;
        foreach (Layer layer in layers) 
            sum += layer.weight;

        return sum;
    }

    public List<Vector2Int> getMoveLocations(Vector2Int gridPoint, bool isQuant) {
        Piece piece = getPieceAtGrid(gridPoint);
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        for (int i = 0; i < layers.Count; i++) {
            List<Vector2Int> allowedGridsInLayer = layers[i].getMoveLocationsInLayerInStep(piece, gridPoint, isQuant);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Commn.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }
    public List<Vector2Int> getMoveLocations(Vector2Int startPoint, Vector2Int midPoint) {
        Piece piece = getPieceAtGrid(startPoint);
        List<Vector2Int> allowedGrids = new List<Vector2Int>();

        for (int i = 0; i < layers.Count; i++) {
            List<Vector2Int> allowedGridsInLayer = layers[i].getMoveLocationsInLayerInMid(piece, startPoint, midPoint);
            for (int j = 0; j < allowedGridsInLayer.Count; j++)
                Commn.AddLocation(allowedGridsInLayer[j], allowedGrids);
        }

        return allowedGrids;
    }

    public bool isQuantumMovePos(Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        Piece piece = getPieceAtGrid(startGridPoint);

        int sumOfLayersWeight = 0;
        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInMid(piece, startGridPoint, midGridPoint, finishGridPoint)) {
                sumOfLayersWeight += layer.weight;
            } else {
                sumOfLayersWeight += layer.weight * 2;
            }
        }

        return sumOfLayersWeight <= limitLayers;
    }
    public void QuantumMove(Vector2Int startGridPoint, Vector2Int midGridPoint, Vector2Int finishGridPoint) {
        Piece piece = getPieceAtGrid(startGridPoint);
        List<Layer> toAddLayers = new List<Layer>();

        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInMid(piece, startGridPoint, midGridPoint, finishGridPoint)) {
                toAddLayers.Add(new Layer(layer));
                layer.moveFromTo(startGridPoint, finishGridPoint);
            } else {
                layer.weight *= 2;
            }
        }

        layers.AddRange(toAddLayers);

        afterMove(finishGridPoint);
    }
    public void SimpleMove(Vector2Int startGridPoint, Vector2Int finishGridPoint) {
        Piece piece = getPieceAtGrid(startGridPoint);

        foreach (Layer layer in layers) {
            if (layer.isLayerLegalInStep(piece, startGridPoint, finishGridPoint, false))
                layer.moveFromTo(startGridPoint, finishGridPoint);
        }

        afterMove(finishGridPoint);
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

    public void afterMove(Vector2Int lastFinishGridPoint) {
        resolveConflicts(lastFinishGridPoint);
        quantumNormalize();

        GameManager.instance.nextStep(quantumStatePiece, quantumStateCount);
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
    public void quantumNormalize() {
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

                quantumStatePiece[i, j] = piece;
                quantumStateCount[i, j] = count;
            }
        }
    }
}
