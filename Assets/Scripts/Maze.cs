using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int sizeX, sizeZ;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;

    public float generationStepDelay;

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[sizeX, sizeZ];
        for(int x = 0; x < sizeX; ++x)
        {
            for(int y=0; y < sizeZ; ++y)
            {
                yield return delay;
                CreateCell(x, y);
            }
        }
    }

    private void CreateCell(int x, int z)
    {
        MazeCell newCell = Instantiate<MazeCell>(cellPrefab);
        cells[x, z] = newCell;
        newCell.name = "Cell_" + x + "_" + z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition  = new Vector3(x - sizeX * 0.5f + 0.5f, 
            0f, z - sizeZ * 0.5f + 0.5f);
    }
}
