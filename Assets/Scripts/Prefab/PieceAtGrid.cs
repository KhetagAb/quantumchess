using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PieceTypeOnGrid {
    Permanent,
    UntilAct,
    Temp
}

public class PieceAtGrid : MonoBehaviour {
    private GameObject pieceObj;
    private GameObject tile;

    private Piece piece;
    private bool isAlphaPiece;
    private PieceTypeOnGrid type; // 0 - Permanent, 1 - ToNextStep, 2 - Temp

    private bool isTileOn;
    private Material[] tileMat = new Material[2]; // Select Tile, Permanent Tile

    // =======================
    private GameObject qdObj;
    private int? qd;
    public void setQD(int? qd) {
        if (this.qd == qd)
            return;

        if (qd == null) {
            Destroy(qdObj);
            this.qd = null;
            qdObj = null;
        } else {
            if (this.qd == null)
                qdObj = Instantiate(Prefabs.instance.debugText, transform);

            this.qd = qd;
            qdObj.GetComponent<TextMeshPro>().text = qd.ToString() + "%";
        }
    }
    public void lookat(Vector3 to) {
        if (qdObj != null)
            qdObj.transform.LookAt(to);
    }
    // =======================

    private PieceAtGrid() {
        isTileOn = false;
    }

    public void changeTypeToUntilPiece() {
        if (type == PieceTypeOnGrid.Permanent)
            return;

        type = PieceTypeOnGrid.UntilAct;
    }

    public void setPiece(Piece piece, bool isAlpha, PieceTypeOnGrid type) {
        if (this.piece == piece)
            return;

        delPiece();

        addPiece(piece, isAlpha, type);
    }
    public void addPiece(Piece piece, bool isAlpha, PieceTypeOnGrid type) {
        if (this.piece != null || piece == null)
            return;

        this.piece = piece;
        this.isAlphaPiece = isAlpha;
        this.type = type;

        createPiece();
    }
    private void createPiece() {
        if (this.piece == null)
            return;

        pieceObj = Instantiate(piece.getPrefab(isAlphaPiece), transform);
        if (piece.colorOfPiece == PlayerColor.Black && piece.typeOfPiece == PieceType.Knight)
            pieceObj.transform.Rotate(Vector3.up, 180);

        updateTile();
    }
    public void delNotPermPiece() {
        if (type == PieceTypeOnGrid.Permanent)
            return;

        delPiece();
    }
    public void delPiece() {
        if (pieceObj != null)
            Destroy(pieceObj);
        piece = null;
        pieceObj = null;
        isAlphaPiece = false;

        updateTile();
    }
    
    public void setTile() {
        isTileOn = true;
        updateTile();
    }
    public void delTile() {
        isTileOn = false;
        updateTile();
    }

    public void setSelector(Piece piece = null) {
        if (isTileOn)
            addPiece(piece, true, PieceTypeOnGrid.Temp);

        tileMat[0] = Prefabs.instance.selectTile;

        initTile();
    }
    public void delSelector() {
        if (type == PieceTypeOnGrid.Temp)
            delPiece();

        tileMat[0] = null;

        initTile();
    }

    private void updateTile() {
        if (isTileOn)
            tileMat[1] = Prefabs.instance.getTileForPiece(piece, isAlphaPiece);
        else
            tileMat[1] = null;

        initTile();
    }
    private void initTile() {
        Material temp = null;

        for (int i = 0; i < tileMat.Length; i++) {
            if (tileMat[i] != null) {
                temp = tileMat[i];
                break;
            }
        }

        if (temp != null) {
            if (tile == null)
                tile = Instantiate(Prefabs.instance.tile, transform);

            tile.GetComponent<MeshRenderer>().material = temp;
        } else {
            if (tile != null) {
                Destroy(tile);
                tile = null;
            }
        }
    }

    public void selectSimplePieceAtGrid() {
        Material toSet = Prefabs.instance.getSimpelSel(piece.colorOfPiece);
        foreach (MeshRenderer mesh in pieceObj.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;
    }
    public void selectQuantumPieceAtGrid() {
        Material toSet = Prefabs.instance.getQuantSel(piece.colorOfPiece);
        foreach (MeshRenderer mesh in pieceObj.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;
    }
    public void deselectPieceAtGrid() {
        Material toSet = Prefabs.instance.getDefault(piece.colorOfPiece);
        foreach (MeshRenderer mesh in pieceObj.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;
    }
}
