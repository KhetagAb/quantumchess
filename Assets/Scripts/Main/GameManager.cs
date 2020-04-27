using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [SerializeField] private Text showCurrentPlayer;

    private int k = 1;
    private Player white;
    private Player black;
    public Player curPlayer { get { return (k == 0) ? white : black; }  }

    public void nextStep(Piece[,] board, int[,] quantums) {
        Display.instance.ToNextStep(board, quantums);

        k ^= 1;
        showCurrentPlayer.text = "current player: " + curPlayer.color.ToString().ToLower();
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
        Step.instance.layers.Add(new Layer());

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

        Step.instance.afterMove(new Vector2Int(0, 0));
    }
    private void InstallSetPiece(Piece piece, int col, int row) {
        Step.instance.layers[0].pieces[col, row] = piece;
    }
}
