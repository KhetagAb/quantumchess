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

    [SerializeField] public Material defaultWhite;
    [SerializeField] public Material defaultBlack;
    [SerializeField] public Material defaultWhiteAlpha;
    [SerializeField] public Material defaultBlackAlpha;
    [SerializeField] public Material simpleWhiteSel;
    [SerializeField] public Material simpleBlackSel;
    [SerializeField] public Material quantumWhiteSel;
    [SerializeField] public Material quantumBlackSel;

    [SerializeField] public GameObject selectTile;
    [SerializeField] public GameObject allowedTile;
    [SerializeField] public GameObject enemyTile;
    [SerializeField] public GameObject midTile;

    [SerializeField] public Material allowCastle;
    [SerializeField] public Material denyCastle;
    [SerializeField] public Material defaultCastle;

    [SerializeField] public GameObject prefKing;
    [SerializeField] public GameObject prefQueen;
    [SerializeField] public GameObject prefKnight;
    [SerializeField] public GameObject prefBishop;
    [SerializeField] public GameObject prefRook;
    [SerializeField] public GameObject prefPawn;
}
