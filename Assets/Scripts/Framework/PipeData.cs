using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeData {
    public enum PipeType {
        Void,
        Corner,
        Straight,
        Cross,
        T
    }

    public static Dictionary<PipeType, int> numberOfConnections = new Dictionary<PipeType, int>()
    {
        {PipeType.Void, 0 },
        {PipeType.Corner, 2 },
        {PipeType.Cross, 4 },
        {PipeType.Straight, 2 },
        {PipeType.T, 3 }
    };

    public static Dictionary<PipeType, List<GameData.Direction>> defaultConnectionOrientation = new Dictionary<PipeType, List<GameData.Direction>>()
    {
        {PipeType.Void, new List<GameData.Direction>() },
        {PipeType.Corner, new List<GameData.Direction>() {GameData.Direction.North, GameData.Direction.East} },
        {PipeType.Cross, new List<GameData.Direction>() {GameData.Direction.East, GameData.Direction.North, GameData.Direction.South, GameData.Direction.West} },
        {PipeType.Straight, new List<GameData.Direction>() {GameData.Direction.North, GameData.Direction.South} },
        {PipeType.T, new List<GameData.Direction>() {GameData.Direction.West, GameData.Direction.East, GameData.Direction.South} }
    };
}
