using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleAlphaPiece : MonoBehaviour {
    protected Vector2Int kingAlphaGrid;
    protected Vector2Int rookAlphaGrid;

    protected GameObject kingAlpha;
    protected GameObject rookAlpha;

    protected GameObject allowedTile;

    protected void showAlphaAndTile() {
        GameObject tempPrefab = PrefabIndexing.instance.getCorrectTile(kingAlphaGrid);
        allowedTile = Instantiate(tempPrefab, Geometry.PointFromGrid(kingAlphaGrid), tempPrefab.transform.rotation, GameManager.instance.BoardObjectOnScene.transform);

        if (GameManager.instance.getPieceIDAtGrid(kingAlphaGrid) == null)
            kingAlpha.SetActive(true);

        if (GameManager.instance.getPieceIDAtGrid(rookAlphaGrid) == null)
            rookAlpha.SetActive(true);
    }
    protected void hideAlphaAndTile() {
        Destroy(allowedTile);

        kingAlpha.SetActive(false);
        rookAlpha.SetActive(false);
    }
}
