using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { White, Black };

public class Player {
    public PlayerType name;

    public List<int> playersPieces;

    public int ZAxis;

    public Player(PlayerType name, bool isWhite) {
         this.name = name;

        playersPieces = new List<int>();

        if (isWhite)
            ZAxis = 1;
        else
            ZAxis = -1;
    }
}
