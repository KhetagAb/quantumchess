using System.Collections.Generic;
using UnityEngine;

public enum PlayerColor { White, Black };

public class Player {
    public PlayerColor color;

    public int ZAxis;
    public Player(PlayerColor color) {
         this.color = color;

        if (this.color == PlayerColor.White)
            ZAxis = 1;
        else
            ZAxis = -1;
    }
}
