using System;
using System.Collections.Generic;
using UnityEngine;

public class CastleInLayer {
    public Piece[,] pieces;
    public string rang;

    public bool[] isCastleAllow = new bool[] { true, true, true, true }; // [0]whiteShort, [1]whiteLong, [2]blackShort, [3]blackLong; 
    private bool betweenResolvingIsLegal = false;

    protected CastleInLayer() { }
    protected CastleInLayer(CastleInLayer layer) {
        for (int i = 0; i < isCastleAllow.Length; i++)
            isCastleAllow[i] = layer.isCastleAllow[i];
    }

    // Массивы ячеек королей и ладей, ячеек между ними и ячеек перемещения рокировки
    private static List<Vector2Int> rookPlaces = new List<Vector2Int>() {
        new Vector2Int(7, 0), new Vector2Int(0, 0),
        new Vector2Int(7, 7), new Vector2Int(0, 7) }; // [0] [1] [2] [3]
    private static List<Vector2Int> kingPlaces = new List<Vector2Int>() {
        new Vector2Int(4, 0), new Vector2Int(4, 7) }; // [0-1] [1-2]
    private static List<Vector2Int>[] betweenCastling = new List<Vector2Int>[] {
        new List<Vector2Int>() { new Vector2Int(5, 0), new Vector2Int(6, 0) },                          // [0]
        new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0) },    // [1]
        new List<Vector2Int>() { new Vector2Int(5, 7), new Vector2Int(6, 7) },                          // [2]
        new List<Vector2Int>() { new Vector2Int(1, 7), new Vector2Int(2, 7), new Vector2Int(3, 7) }};   // [3]
    private static List<Step> rookMoves = new List<Step> {
        new Step(rookPlaces[0], betweenCastling[0][0]),                                 // [0]
        new Step(rookPlaces[1], betweenCastling[1][2]),                                 // [1]
        new Step(rookPlaces[2], betweenCastling[2][0]),                                 // [2]
        new Step(rookPlaces[3], betweenCastling[3][2])                                  // [3]
    };
    private static List<Step> kingMoves = new List<Step> {
        new Step(kingPlaces[0], betweenCastling[0][1]),                                 // [0]
        new Step(kingPlaces[0], betweenCastling[1][1]),                                 // [1]
        new Step(kingPlaces[1], betweenCastling[2][1]),                                 // [2]
        new Step(kingPlaces[1], betweenCastling[3][1])                                  // [3]
    };

    public void calRang() {
        rang = "";

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (pieces[i, j] == null)
                    rang = rang + 'n';
                else
                    rang = rang + (char) ('a' + (int) pieces[i, j].typeOfPiece + (pieces[i, j].colorOfPiece == PlayerColor.Black ? 6 : 0));
            }
        }

        for (int i = 0; i < 4; i++)
            rang = rang + (char) ('a' + (isCastleAllow[i] ? 1 : 0));
    }

    // ===================================================[FUNCTIONAL] 
    public static bool isCorrectGrid(Vector2Int gridPoint) {
        int col = gridPoint.x, row = gridPoint.y;
        return (0 <= col && col <= 7 && 0 <= row && row <= 7);
    }

    //  ===================================================[LAYER] 
    public Piece getPieceAtGrid(Vector2Int gridPoint) {
        if (!isCorrectGrid(gridPoint))
            return null;

        return pieces[gridPoint.x, gridPoint.y];
    }
    public PieceType? getPieceTypeAtGrid(Vector2Int gridPoint) {
        Piece piece = getPieceAtGrid(gridPoint);
        if (piece == null)
            return null;

        return piece.typeOfPiece;
    }
    protected void setFromTo(Step step) {
        int cols = step.from.x, rows = step.from.y;
        int colf = step.to.x, rowf = step.to.y;
        pieces[colf, rowf] = pieces[cols, rows];
        pieces[cols, rows] = null;
    }

    // ===================================================[CASTLE] 
    public void castleKing(int index) {
        betweenResolvingIsLegal = true;
        setFromTo(kingMoves[index]);
    }
    public void castleRook(int index) {
        setFromTo(rookMoves[index]);
        normalizeCastleInLayer();
    }
    protected void normalizeCastleInLayer() {
        for (int i = 0; i < 4; i++) {
            if (getPieceTypeAtGrid(rookMoves[i].from) != PieceType.Rook)
                isCastleAllow[i] = false;
        }

        for (int i = 0; i < 4; i += 2) {
            if (getPieceTypeAtGrid(kingMoves[i].from) != PieceType.King)
                isCastleAllow[i] = isCastleAllow[i + 1] = false;
        }
        calRang();
    }
    public bool isCastleLegal(int index) {
        if (betweenResolvingIsLegal) {
            betweenResolvingIsLegal = false;
            return true;
        }

        bool isFreeBetween = isCastleAllow[index];
        foreach (Vector2Int grid in betweenCastling[index]) {
            isFreeBetween = isFreeBetween && (getPieceAtGrid(grid) == null);
        }

        return isFreeBetween;
    }
}
