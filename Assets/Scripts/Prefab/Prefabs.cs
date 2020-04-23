using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Prefabs : MonoBehaviour {
    public static Prefabs instance;

    private Prefabs() {
        instance = this;
    }

    [SerializeField] public GameObject piece;
    [SerializeField] public GameObject debugText;

    [SerializeField] private Material defaultWhite;
    [SerializeField] private Material defaultBlack;
    [SerializeField] private Material defaultWhiteAlpha;
    [SerializeField] private Material defaultBlackAlpha;
    [SerializeField] private Material simpleWhiteSel;
    [SerializeField] private Material simpleBlackSel;
    [SerializeField] private Material quantumWhiteSel;
    [SerializeField] private Material quantumBlackSel;

    [SerializeField] public GameObject tile;
    [SerializeField] public Material selectTile;
    [SerializeField] private Material clearTile;
    [SerializeField] private Material pieceTile;
    [SerializeField] private Material alphaTile;

    [SerializeField] public Material allowCastle;
    [SerializeField] public Material denyCastle;
    [SerializeField] public Material defaultCastle;

    [SerializeField] public GameObject prefKing;
    [SerializeField] public GameObject prefQueen;
    [SerializeField] public GameObject prefKnight;
    [SerializeField] public GameObject prefBishop;
    [SerializeField] public GameObject prefRook;
    [SerializeField] public GameObject prefPawn;

    public Material getTileForPiece(Piece piece, bool isAlpha = false) {
        if (isAlpha) {
            return alphaTile;
        } else if (piece == null) {
            return clearTile;
        } else {
            return pieceTile;
        }
    }

    public Material getDefault(PlayerColor color) {
        if (color == PlayerColor.White)
            return defaultWhite;
        else
            return defaultBlack;
    }
    public Material getAlpha(PlayerColor color) {
        if (color == PlayerColor.White)
            return defaultWhiteAlpha;
        else
            return defaultBlackAlpha;
    }
    public Material getSimpelSel(PlayerColor color) {
        if (color == PlayerColor.White)
            return simpleWhiteSel;
        else
            return simpleBlackSel;
    }
    public Material getQuantSel(PlayerColor color) {
        if (color == PlayerColor.White)
            return quantumWhiteSel;
        else
            return quantumBlackSel;
    }
}
