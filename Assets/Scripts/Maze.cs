using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Growing Tree algorithm
 * http://weblog.jamisbuck.org/2011/1/27/maze-generation-growing-tree-algorithm
 */


public class Maze : MonoBehaviour
{
    public Vector2Int size;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall[] wallPrefabs;
    public MazeDoor doorPrefab;

    [Range(0f, 1f)]
    public float doorProbability;

    private IMazeGenerationMethod mazeGenerationMethod;
    public void SetMazeGenerationMethod(MazeGenerationMethod method)
    {

        switch(method)
        {
            case MazeGenerationMethod.Last:
                mazeGenerationMethod = new MazeGenerationLast();
                break;
            case MazeGenerationMethod.Random:
                mazeGenerationMethod = new MazeGenerationRandom();
                break;
            case MazeGenerationMethod.Middle:
                mazeGenerationMethod = new MazeGenerationMiddle();
                break;
            case MazeGenerationMethod.First:
                mazeGenerationMethod = new MazeGenerationFirst();
                break;
        }
    }
    
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
        List<MazeCell> activeCells = new List<MazeCell>(cells.Length);      // activeCells

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

    #region Maze generation method strategy

    interface IMazeGenerationMethod
    {
        int GetCurrentIndex(List<MazeCell> activeCells);
    }

    class MazeGenerationLast : IMazeGenerationMethod
    {
        public int GetCurrentIndex(List<MazeCell> activeCells)
        {
            return activeCells.Count - 1;
        }
    }

    class MazeGenerationFirst : IMazeGenerationMethod
    {
        public int GetCurrentIndex(List<MazeCell> activeCells)
        {
            return 0;
        }
    }

    class MazeGenerationMiddle : IMazeGenerationMethod
    {
        public int GetCurrentIndex(List<MazeCell> activeCells)
        {
            return activeCells.Count / 2;
        }
    }

    class MazeGenerationRandom : IMazeGenerationMethod
    {
        public int GetCurrentIndex(List<MazeCell> activeCells)
        {
            return Random.Range(0, activeCells.Count - 1);
        }
    }

    #endregion

    // change the nature of the maze you generate by using a different method to select the current index
    private int GetCurrentIndex(List<MazeCell> activeCells)
    {
        return mazeGenerationMethod.GetCurrentIndex(activeCells);
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        
        int currentIndex = GetCurrentIndex(activeCells);
        MazeCell currentCell = activeCells[currentIndex];
        if(currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        Vector2Int coordinates = currentCell.coordinates + direction.ToVector2Int();

        //Debug.Log("DoNextGenerationStep index: " + currentIndex + ", cell: " + currentCell
        //    + ", direction" + direction.ToString() + ", coords: " + coordinates.ToString());

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
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
        }
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        //Debug.Log("CreatePassage " + cell.name + ", " + otherCell.name + ", " + direction.ToString());
        MazePassage prefab = (Random.value < doorProbability) ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate<MazePassage>(prefab);
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate<MazePassage>(passagePrefab);  // is this good 2 times?
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        //Debug.Log("CreateWall " + cell.name + ", " + otherCell?.name + ", " + direction.ToString());
        MazeWall wall = Instantiate<MazeWall>(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
        wall.Initialize(cell, otherCell, direction);
        if(otherCell != null)
        {
            wall = Instantiate<MazeWall>(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
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
