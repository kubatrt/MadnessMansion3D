using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection
{
    North,
    East,
    South,
    West
}

public static class MazeDirections
{
    public const int Count = 4;

    private static Vector2Int[] vectors =
    {
        new Vector2Int(0, 1),   // North
        new Vector2Int(1, 0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };

    // Convert direction to Vector2Int (extension method)
    public static Vector2Int ToVector2Int(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

    public static MazeDirection RandomValue
    {
        get
        {
            MazeDirection md = (MazeDirection)Random.Range(0, Count);
            return md;
        }
    }
}

