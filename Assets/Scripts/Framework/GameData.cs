using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class GameData
    {

        public enum Team
        {
            Red,
            Blue,
            Yellow,
            Black,
            Neutral
        }

        public enum Direction
        {
            North,
            East,
            South,
            West
        }

        public enum PlayerSourceDirection
        {
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
        {Team.Yellow, Color.yellow },
        {Team.Neutral, Color.white }
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

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                Coordinate c = obj as Coordinate;
                if ((System.Object)c == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (x == c.x) && (y == c.y);
            }
        }


    }

