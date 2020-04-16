using System.Collections.Generic;
using UnityEngine;

public class PrefabIndexing : MonoBehaviour {
    public static PrefabIndexing instance;

    private PrefabIndexing() {
        instance = this;
    }

    private static List<GameObject> idToPrefab;
    private static List<GameObject> idToPrefabAlpha;

    [SerializeField] public Material defaultMaterial;
    [SerializeField] public Material simpleSelWhiteMaterial;
    [SerializeField] public Material simpleSelBlackMaterial;
    [SerializeField] public Material quantSelWhiteMaterial;
    [SerializeField] public Material quantSelBlackMaterial;
    [SerializeField] public Material enemyTile;
    [SerializeField] public Material allowedTile;
    [SerializeField] public Material selectTile;
    [SerializeField] public Material unselectRoque;

    [SerializeField] public GameObject prefSelectTile;
    [SerializeField] public GameObject prefAllowedTile;
    [SerializeField] public GameObject prefEnemyTile;
    [SerializeField] public GameObject prefMidTile;

    [SerializeField] private GameObject prefWhiteKing;      // 0
    [SerializeField] private GameObject prefWhiteQueen;     // 1
    [SerializeField] private GameObject prefWhiteKnight;    // 2
    [SerializeField] private GameObject prefWhiteBishop;    // 3
    [SerializeField] private GameObject prefWhiteRook;      // 4
    [SerializeField] private GameObject prefWhitePawn;      // 5
    [SerializeField] private GameObject prefBlackKing;      // 6
    [SerializeField] private GameObject prefBlackQueen;     // 7
    [SerializeField] private GameObject prefBlackKnight;    // 8
    [SerializeField] private GameObject prefBlackBishop;    // 9
    [SerializeField] private GameObject prefBlackRook;      // 10
    [SerializeField] private GameObject prefBlackPawn;      // 11

    [SerializeField] private GameObject prefWhiteKingAlpha;      // 0
    [SerializeField] private GameObject prefWhiteQueenAlpha;     // 1
    [SerializeField] private GameObject prefWhiteKnightAlpha;    // 2
    [SerializeField] private GameObject prefWhiteBishopAlpha;    // 3
    [SerializeField] public GameObject prefWhiteRookAlpha;      // 4
    [SerializeField] private GameObject prefWhitePawnAlpha;      // 5
    [SerializeField] private GameObject prefBlackKingAlpha;      // 6
    [SerializeField] private GameObject prefBlackQueenAlpha;     // 7
    [SerializeField] private GameObject prefBlackKnightAlpha;    // 8
    [SerializeField] private GameObject prefBlackBishopAlpha;    // 9
    [SerializeField] public GameObject prefBlackRookAlpha;      // 10
    [SerializeField] private GameObject prefBlackPawnAlpha;      // 11

    private void  Awake() {
        idToPrefab = new List<GameObject>() {
            prefWhiteKing, prefWhiteQueen, prefWhiteKnight, prefWhiteBishop, prefWhiteRook, prefWhitePawn,
            prefBlackKing, prefBlackQueen, prefBlackKnight, prefBlackBishop, prefBlackRook, prefBlackPawn };
        idToPrefabAlpha = new List<GameObject>() {
            prefWhiteKingAlpha, prefWhiteQueenAlpha, prefWhiteKnightAlpha, prefWhiteBishopAlpha, prefWhiteRookAlpha, prefWhitePawnAlpha,
            prefBlackKingAlpha, prefBlackQueenAlpha, prefBlackKnightAlpha, prefBlackBishopAlpha, prefBlackRookAlpha, prefBlackPawnAlpha };
    }
    public static readonly Piece[] pieceClasses = {
        new King(PieceType.King), new Queen(PieceType.Queen),
        new Knight(PieceType.Knight), new Bishop(PieceType.Bishop),
        new Rook(PieceType.Rook), new Pawn(PieceType.Pawn) };
    public static readonly Piece[] idToClass = { 
        pieceClasses[0], pieceClasses[1], pieceClasses[2], pieceClasses[3], pieceClasses[4], pieceClasses[5],
        pieceClasses[0], pieceClasses[1], pieceClasses[2], pieceClasses[3], pieceClasses[4], pieceClasses[5]};
    public static readonly PieceType[] idToType = {
        PieceType.King, PieceType.Queen, PieceType.Knight, PieceType.Bishop, PieceType.Rook, PieceType.Pawn,
        PieceType.King, PieceType.Queen, PieceType.Knight, PieceType.Bishop, PieceType.Rook, PieceType.Pawn};
    public static Piece getPieceClass(int ID) {
        return idToClass[ID];
    }
    public static PieceType getPieceTypeByID(int ID) {
        return idToType[ID];
    }
    public static GameObject getPrefabByID(int ID) {
        return idToPrefab[ID];
    }
    public static GameObject getPrefabAlphaByID(int ID) {
        return idToPrefabAlpha[ID];
    }
}
