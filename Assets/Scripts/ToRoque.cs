using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToRoque : MonoBehaviour {
    [SerializeField] private Button shortCastling;
    [SerializeField] private Button longCastling;

    // [0]WhiteShort, [1]WhiteLong, [2]BlackShort, [3]BlackLong;
    private bool[] roques = new bool[] { true, true, true, true };
    private static List<Vector2Int>[] roqueFromTo = {
        new List<Vector2Int>() { new Vector2Int(7, 0), new Vector2Int(5, 0), new Vector2Int(4, 0), new Vector2Int(6, 0) },      // 0
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(3, 0), new Vector2Int(4, 0), new Vector2Int(2, 0) },      // 1
        new List<Vector2Int>() { new Vector2Int(7, 7), new Vector2Int(5, 7), new Vector2Int(4, 7), new Vector2Int(6, 7) },      // 2
        new List<Vector2Int>() { new Vector2Int(0, 7), new Vector2Int(3, 7), new Vector2Int(4, 7), new Vector2Int(2, 7) }};     // 3

    public void roqueNormalize() {
        for (int j = 0; j < roques.Length; j++)
            roques[j] = false;

        for (int i = 0; i < GameManager.layers.Count; i++) {
            bool[] inCurLayer = GameManager.layers[i].getRoqueStatus();

            for (int j = 0; j < roques.Length; j++)
                roques[j] = roques[j] || inCurLayer[j];
        }
    }

    public void updateTheButtonsStatus() {
        if (GameManager.instance.currentPlayer.name == PlayerType.White) {
            shortCastling.interactable = roques[0];
            longCastling.interactable = roques[1];
        } else {
            shortCastling.interactable = roques[2];
            longCastling.interactable = roques[3];
        }
    }

    public void toRoque(bool isLong) {
        StepQuantumSelection SQS = GetComponent<StepQuantumSelection>();
        StepSimpleSelection SSS = GetComponent<StepSimpleSelection>();
        SQS.Disactivate();
        SSS.Disactivate();

        int curPlayer = (GameManager.instance.currentPlayer.name == PlayerType.White ? 0 : 2) + (isLong ? 1 : 0);
        foreach (Layer curLayer in GameManager.layers) {
            if (curLayer.getRoqueStatus()[curPlayer]) {
                for (int i = 0; i < roqueFromTo[curPlayer].Count; i += 2)
                    curLayer.setFromTo(roqueFromTo[curPlayer][i], roqueFromTo[curPlayer][i + 1]);
            }
        }

        GameManager.instance.quantumNormalize();

        StepAndBoardDisplay goTo = GetComponent<StepAndBoardDisplay>();
        goTo.EnterState();
    }
}
