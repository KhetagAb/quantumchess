using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastleArrow : CastleAlphaPiece, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private int index;
    private bool isDeny;

    // Хранилище префабов по ID'шникам
    public static GameObject[] prefabKingAlpha; // whiteShort, whiteLong, blackShort, blackLong
    public static GameObject[] prefabRookAlpha; // whiteShort, whiteLong, blackShort, blackLong

    // Массивы координат установки
    public static List<Vector2Int> kingAlphaToGrids = new List<Vector2Int>() {
        new Vector2Int(6, 0), new Vector2Int(2, 0), new Vector2Int(6, 7), new Vector2Int(2, 7) };
    public static List<Vector2Int> rookAlphaToGrids = new List<Vector2Int>() {
        new Vector2Int(5, 0), new Vector2Int(3, 0), new Vector2Int(5, 7), new Vector2Int(3, 7) };

    private void Awake() {
        prefabKingAlpha = new GameObject[] {
            new King(PlayerColor.White).getPrefab(), new King(PlayerColor.White).getPrefab(),
            new King(PlayerColor.Black).getPrefab(), new King(PlayerColor.Black).getPrefab() };
        prefabRookAlpha = new GameObject[] {
           new Rook(PlayerColor.White).getPrefab(), new Rook(PlayerColor.White).getPrefab(),
           new Rook(PlayerColor.Black).getPrefab(), new Rook(PlayerColor.Black).getPrefab()};
    }

    private void Start() {
        kingAlphaGrid = kingAlphaToGrids[index];
        rookAlphaGrid = rookAlphaToGrids[index];

        GameObject tempprefab = prefabKingAlpha[index];
        kingAlpha = Instantiate(tempprefab, Geometry.PointFromGrid(kingAlphaGrid), tempprefab.transform.rotation, GameManager.instance.BoardObjectOnScene.transform);
        tempprefab = prefabRookAlpha[index];
        rookAlpha = Instantiate(tempprefab, Geometry.PointFromGrid(rookAlphaGrid), tempprefab.transform.rotation, GameManager.instance.BoardObjectOnScene.transform);

        hideCastle();
    }

    public void setDenyStatus(bool status) {
        isDeny = status;

        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        if (!isDeny)
            mesh.material = Prefabs.instance.defaultCastle;
        else
            mesh.material = Prefabs.instance.denyCastle;
    }

    public void showCastle() {
        gameObject.SetActive(true);
    }

    public void hideCastle() {
        hideAlphaAndTile();
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!isDeny) {
            Castling.instance.Castle(index);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isDeny) {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            mesh.material = Prefabs.instance.allowCastle;

            showAlphaAndTile();
        }
    }
    public void OnPointerExit(PointerEventData eventData) {
        if (!isDeny) {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            mesh.material = Prefabs.instance.defaultCastle;

            hideAlphaAndTile();
        }
    }
}
