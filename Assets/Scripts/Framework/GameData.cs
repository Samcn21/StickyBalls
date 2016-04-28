using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class GameData
    {

        public enum Team
        {
            Purple,
            Blue,
            Yellow,
            Cyan,
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

        {Team.Cyan,          new Color32(0, 255, 255 , 1) },
        {Team.Purple,        new Color32(255, 0, 255 , 1) },
        {Team.Yellow,        new Color32(255, 215, 0 , 1) },
        {Team.Blue,          new Color32(0, 50, 200 , 1) },
        {Team.Neutral,       new Color32(232, 232, 232 , 1) }
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

