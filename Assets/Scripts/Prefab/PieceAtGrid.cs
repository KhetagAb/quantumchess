using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceAtGrid : MonoBehaviour {
    private Piece piece;
    private bool isAlpha;
    private GameObject pieceObj;

    private int? qd;
    private GameObject qdObj;

    private bool isPerm;
    private GameObject permTile;
    private GameObject selTile;

    public void setPiece(Piece piece, bool isAlpha = false, bool isPerm = true) {
        if (this.piece == piece)
            return;

        delPiece();

        addPiece(piece, isAlpha, isPerm);
    }
    public void addPiece(Piece piece, bool isAlpha = false, bool isPerm = true) {
        if (this.piece != null || piece == null)
            return;

        this.piece = piece;
        this.isAlpha = isAlpha;
        this.isPerm = isPerm;

        createPiece();
    }
    private void createPiece() {
        pieceObj = Instantiate(piece.getPrefab(isAlpha), transform);
        if (piece.colorOfPiece == PlayerColor.Black && piece.typeOfPiece == PieceType.Knight)
            pieceObj.transform.Rotate(Vector3.up, 180);

        updatePermTile();
    }
    public void setPermPiece(Piece piece, bool isAlpha = false, bool isPerm = true) {
        addPiece(piece, isAlpha, isPerm);
        this.isPerm = true;
    }
    public void delPiece() {
        Destroy(pieceObj);
        piece = null;
        pieceObj = null;
        isAlpha = false;
        isPerm = false;

        updatePermTile();
    }
    public void delNotPermPiece() {
        if (isPerm)
            return;

        delPiece();
    }
    public void delAlphaPiece() {
        if (!isAlpha)
            return;

        delPiece();
    }

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

    private void showPermTile() {
        permTile.SetActive(true);
    }
    private void hidePermTile() {
        permTile.SetActive(false);
    }
    public void setPermTile() {
        if (permTile == null)
            permTile = Instantiate(Prefabs.instance.tile, transform);
        updatePermTile();
    }
    public void updatePermTile() {
        if (permTile != null)
            permTile.GetComponent<MeshRenderer>().material = Prefabs.instance.getTileForPiece(piece, isAlpha);
    }
    public void delPermTile() {
        Destroy(permTile);
        permTile = null;
    }

    public void setSelTile(Piece piece = null) {
        if (permTile != null) {
            hidePermTile();
            if (piece != null && !isPerm) {
                addPiece(piece, true, false);
            }
        }

        if (selTile == null)
            selTile = Instantiate(Prefabs.instance.tile, transform);

        selTile.GetComponent<MeshRenderer>().material = Prefabs.instance.selectTile;
    }
    public void delSelTile() {
        Destroy(selTile);
        selTile = null;

        if (permTile != null) {
            showPermTile();
            delNotPermPiece();
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
