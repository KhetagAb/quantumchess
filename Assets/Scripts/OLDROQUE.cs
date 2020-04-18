using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// хорошо бы было это все переписать или легче нельзя? БОЛЬШАЯ ТУТ БЕДА!
public class ToRoque {
    /*
    // Массив наличия по всем слоям фигур между позициями рокировки
    private bool[] isClearBetween = new bool[] { false, false, false, false };

    public void TryActivate() {
        // [0]WhiteShort, [1]WhiteLong, [2]BlackShort, [3]BlackLong;
        bool[] roquesPieces = new bool[] { false, false, false, false };
        isClearBetween = new bool[] { false, false, false, false };

        for (int i = 0; i < GameManager.layers.Count; i++) {
            bool[] inCurLayer = GameManager.layers[i].getRoqueStatus();

            for (int j = 0; j < roquesPieces.Length; j++) {
                roquesPieces[j] = roquesPieces[j] || GameManager.layers[i].roques[j];
                isClearBetween[j] = isClearBetween[j] || inCurLayer[j];
            }
        }

        int playerIndex = (GameManager.instance.currentPlayer.name == PlayerType.White ? 0 : 2);

        for (int i = 0; i < 2; i++) {
            if (roquesPieces[playerIndex + i]) {
              //  if (isClearBetween[playerIndex + i])
                    //setDefaultRoque(playerIndex + i);
              //  else
                    //setDenyRoque(playerIndex + i);

                //activateRoque(playerIndex + i, true);
            }
        }

    //    this.enabled = true;
    }

    public void Disactivate() {
       // if (!this.enabled)
       //     return;
            
      //  this.enabled = false;

       // Destroy(allowedTileObj);
        allowedTile = null;

        for (int i = 0; i < isClearBetween.Length; i++) {
           // if (isClearBetween[i])
                //setDefaultRoque(i);
        }
        //disactivateAllRoques();

       // StepAndBoardDisplay goTo = GetComponent<StepAndBoardDisplay>();
        //goTo.Activate();
    }
    */
}
 