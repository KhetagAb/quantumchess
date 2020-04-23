using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceAtGrid : MonoBehaviour {
    private Piece piece;
    private GameObject obj;
    private GameObject text;

    public void updatePiece(Piece newPiece) {
        if (newPiece != piece) {
            Destroy(obj);

            piece = newPiece;

            if (piece != null) {
                obj = Instantiate(piece.getPrefab(), transform);
                if (piece.colorOfPiece == PlayerColor.Black && piece.typeOfPiece == PieceType.Knight)
                    obj.transform.Rotate(Vector3.up, 180);
            }
        }
    }
    public void updateDebugText(int? qd) {
        if (qd == null) {
            Destroy(text);
        } else {
            if (text == null)
                text = Instantiate(Prefabs.instance.debugText, transform);

            text.GetComponent<TextMeshPro>().text = qd.ToString() + "%";
        }
    }

    public void selectSimplePieceAtGrid() {
        Material toSet = piece.getSimpleSelection();
        foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;
    }
    public void selectQuantumPieceAtGrid() {
        Material toSet = piece.getQuantumSelection();
        foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;
    }
    public void deselectPieceAtGrid() {
        Material toSet = piece.getDefaultMaterial();
        foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>())
            mesh.material = toSet;
    }
}
