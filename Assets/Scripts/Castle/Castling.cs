using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castling : MonoBehaviour {
    public static Castling instance;

    [SerializeField] private CastleArrow[] castels;

    private void Awake() {
        instance = this;
    }

    public void Castle(int index) {
        StepSimpleSelection SSS = GetComponent<StepSimpleSelection>();
        SSS.Disactivate();
        Disactivate();

        GameManager.instance.castle(index);

        Display goTo = GetComponent<Display>();
        goTo.Activate();
    }

    public void Activate() {
        this.enabled = true;

        bool[] isCastlePieceUntoch = new bool[] { false, false, false, false };
        bool[] isAnyLegal = new bool[] { false, false, false, false };

        int castlePlayer = (GameManager.instance.curPlayer.color == PlayerColor.White ? 0 : 2);
        for (int i = 0; i < GameManager.layers.Count; i++) {
            for (int j = castlePlayer; j - castlePlayer < 2; j++) {
                isCastlePieceUntoch[j] = isCastlePieceUntoch[j] || GameManager.layers[i].isCastlePiecesUntoch[j];
                isAnyLegal[j] = isAnyLegal[j] || GameManager.layers[i].isCastleLegal(j);
            }
        }

        for (int j = castlePlayer; j - castlePlayer < 2; j++) {
            if (isCastlePieceUntoch[j]) {
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
