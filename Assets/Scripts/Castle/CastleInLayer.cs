using System;
using System.Collections.Generic;
using UnityEngine;

public class CastleInLayer {
    protected Layer childLayer;
    public bool[] isCastleAllow; // [0]whiteShort, [1]whiteLong, [2]blackShort, [3]blackLong; 

    private bool betweenResolvingIsLegal;

    protected CastleInLayer() {
        isCastleAllow = new bool[] { true, true, true, true };
        betweenResolvingIsLegal = false;
    }
    protected CastleInLayer(CastleInLayer layer) {
        isCastleAllow = new bool[4];
        betweenResolvingIsLegal = false;

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
        new Step(rookPlaces[0], betweenCastling[0][0]),                                                  // [0]
        new Step(rookPlaces[1], betweenCastling[1][2]),                                // [1]
        new Step(rookPlaces[2], betweenCastling[2][0]),                                // [2]
        new Step(rookPlaces[3], betweenCastling[3][2])                                 // [3]
    };
    private static List<Step> kingMoves = new List<Step> {
        new Step(kingPlaces[0], betweenCastling[0][1]),                              // [0]
        new Step(kingPlaces[0], betweenCastling[1][1]),                                // [1]
        new Step(kingPlaces[1], betweenCastling[2][1]),                                // [2]
        new Step(kingPlaces[1], betweenCastling[3][1])                                 // [3]
    };

    // ===================================================[CASTLE] 
    protected void updateCastleUntochness(Vector2Int startGrid, Vector2Int finishGrid) {
        int findedRook = rookPlaces.FindIndex(grid => grid == startGrid || grid == finishGrid);
        int findedKing = kingPlaces.FindIndex(grid => grid == startGrid || grid == finishGrid);

        if (findedRook != -1)
            isCastleAllow[findedRook] = false;

        if (findedKing != -1) {
            isCastleAllow[2 * findedKing] = false;
            isCastleAllow[2 * findedKing + 1] = false;
        }
    }

    public void castleKing(int index) {
        betweenResolvingIsLegal = true;
        childLayer.setFromTo(kingMoves[index]);
    }
    public void castleRook(int index) {
        childLayer.setFromTo(rookMoves[index]);
    }


    public bool isCastleLegal(int index) {
        if (betweenResolvingIsLegal) {
            betweenResolvingIsLegal = false;
            return true;
        }

        bool isFreeBetween = isCastleAllow[index];

        foreach (Vector2Int grid in betweenCastling[index]) {
            isFreeBetween = isFreeBetween && (childLayer.getPieceAtGrid(grid) == null);
        }

        return isFreeBetween;
    }
}
