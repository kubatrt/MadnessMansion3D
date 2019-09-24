using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vector2Int size;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;

    public float generationStepDelay;

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.y];
        for(int x = 0; x < size.x; ++x)
        {
            for(int z=0; z < size.y; ++z)
            {
                yield return delay;
                CreateCell(new Vector2Int(x,z));
            }
        }
    }

    private void CreateCell(Vector2Int coordinates)
    {
        MazeCell newCell = Instantiate<MazeCell>(cellPrefab);
        cells[coordinates.x, coordinates.y] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Cell_" + coordinates.x + "_" + coordinates.y;
        newCell.transform.parent = transform;
        newCell.transform.localPosition  = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 
            0f, coordinates.y - size.y * 0.5f + 0.5f);
    }
}
