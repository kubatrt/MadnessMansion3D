using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vector2Int size;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

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
        List<MazeCell> activeCells = new List<MazeCell>(cells.Length);
        DoFirstGenerationStep(activeCells);

        while(activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
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

        Debug.Log("DoNextGenerationStep index: " + currentIndex + ", cell: " + currentCell
            + ", direction" + direction.ToString() + ", coords: " + coordinates.ToString());

        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if(neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
                activeCells.RemoveAt(currentIndex);
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
            activeCells.RemoveAt(currentIndex);
        }
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        Debug.Log("CreatePassage " + cell.name + ", " + otherCell.name + ", " + direction.ToString());
        MazePassage passage = Instantiate<MazePassage>(passagePrefab);
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate<MazePassage>(passagePrefab);
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        Debug.Log("CreateWall " + cell.name + ", " + otherCell?.name + ", " + direction.ToString());
        MazeWall wall = Instantiate<MazeWall>(wallPrefab);
        wall.Initialize(cell, otherCell, direction);
        if(otherCell != null)
        {
            wall = Instantiate<MazeWall>(wallPrefab);
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    private MazeCell CreateCell(Vector2Int coordinates)
    {
        MazeCell newCell = Instantiate<MazeCell>(cellPrefab);
        cells[coordinates.x, coordinates.y] = newCell;

        newCell.coordinates = coordinates;
        newCell.name = "Cell_" + coordinates.x + "_" + coordinates.y;
        // why parent?
        newCell.transform.parent = transform;
        newCell.transform.localPosition  = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 
            0f, coordinates.y - size.y * 0.5f + 0.5f);
       
        return newCell;
    }
}
