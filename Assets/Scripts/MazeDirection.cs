using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection
{
    North,  // Z +
    East,   // X +
    South,  // Z -
    West    // X -
}

public static class MazeDirections
{
    public const int Count = 4;

    private static readonly Vector2Int[] vectors =
    {
        new Vector2Int(0, 1),   // North
        new Vector2Int(1, 0),   // East
        new Vector2Int(0,-1),   // South
        new Vector2Int(-1,0)    // West
    };

    public static MazeDirection RandomDirection
    {
        get
        {
            MazeDirection md = (MazeDirection)Random.Range(0, Count);
            return md;
        }
    }

    private static readonly MazeDirection[] opposites = {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return opposites[(int)direction];
    }

    private static readonly Quaternion[] rotations = {
        Quaternion.identity, // default as prefab is
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction];
    }
    
    public static Vector2Int ToVector2Int(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

    public static MazeDirection GetNextClockwise(this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + 1) % Count);
    }
    public static MazeDirection GetNextCounterclockwise(this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + Count - 1) % Count);
    }
}

