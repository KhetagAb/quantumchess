using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castling : MonoBehaviour {
    [SerializeField] private CastleArrow[] castels;

    public void Castle(int index) {
        SimpleSelection SS = GetComponent<SimpleSelection>();
        SS.Disactivate();
        Disactivate();

        GameManager.instance.Castle(index);
    }

    public void Activate() {
        this.enabled = true;

        bool[] isCastleAllow = new bool[] { false, false, false, false };
        bool[] isAnyLegal = new bool[] { false, false, false, false };

        int castlePlayer = (GameManager.instance.curPlayer.color == PlayerColor.White ? 0 : 2);
        foreach (Layer layer in GameManager.instance.layers) {
            for (int j = castlePlayer; j - castlePlayer < 2; j++) {
                isCastleAllow[j] = isCastleAllow[j] || layer.isCastleAllow[j];
                isAnyLegal[j] = isAnyLegal[j] || layer.isCastleLegal(j);
            }
        }

        for (int j = castlePlayer; j - castlePlayer < 2; j++) {
            if (isCastleAllow[j]) {
                castels[j].setDenyStatus(!isAnyLegal[j]);
                castels[j].showCastle();
            }
        }
    }

    public void Disactivate() {
        if (!this.enabled)
            return;

        this.enabled = false;
        for (int i = 0; i < castels.Length; i++) {
            castels[i].hideCastle();
        }
    }
}
