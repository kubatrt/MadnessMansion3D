using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vector2Int size;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;

    public float generationStepDelay;
    public Vector2Int RandomCoordinates
    {
        get
        {
            return new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        }
    }
    public MazeCell GetCell(Vector2Int coordinates)
    {
        return cells[coordinates.x, coordinates.y];
    }

    // Are coordinates in bound of maze?
    public bool ContainsCoordinates(Vector2Int coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x &&
            coordinate.y >= 0 && coordinate.y < size.y;
    }

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.y];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while(activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
            //CreateCell(coordinates);
            //coordinates += MazeDirections.RandomValue.ToVector2Int();
        }
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];

        MazeDirection direction = MazeDirections.RandomValue;
        Vector2Int coordinates = currentCell.coordinates + direction.ToVector2Int();
        // 
        if (ContainsCoordinates(coordinates) && GetCell(coordinates) == null)
        {
            activeCells.Add(CreateCell(coordinates));
        }
        else
        {
            activeCells.RemoveAt(currentIndex);
        }
    }

    private MazeCell CreateCell(Vector2Int coordinates)
    {
        MazeCell newCell = Instantiate<MazeCell>(cellPrefab);
        //cells[coordinates.x, coordinates.y] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Cell_" + coordinates.x + "_" + coordinates.y;
        newCell.transform.parent = transform;
        newCell.transform.localPosition  = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 
            0f, coordinates.y - size.y * 0.5f + 0.5f);
        return newCell;
    }
}
