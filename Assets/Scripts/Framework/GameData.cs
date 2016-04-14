using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData {

    public enum Team
    {
        Red,
        Blue,
        Yellow,
        Black
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public enum PlayerSourceDirection {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public static Dictionary<Team, Color> TeamColors = new Dictionary<Team, Color>() 
    {
        {Team.Red, Color.red },
        {Team.Black, Color.black },
        {Team.Blue, Color.blue },
        {Team.Yellow, Color.yellow }
    };

    public class Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


}
