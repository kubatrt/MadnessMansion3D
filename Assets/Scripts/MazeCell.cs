using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Vector2Int coordinates;
    public MazeRoom room;

    private int initializedEdgesCount;

    public void Initialize(MazeRoom room)
    {
        room.Add(this);
        transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
    }

    public bool IsFullyInitialized
    {
        get
        {
            return initializedEdgesCount == MazeDirections.Count;
        }
    }

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgesCount++;
    }

    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgesCount);
            for(int i=0; i < MazeDirections.Count; i++)
            {
                if(edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }
                    skips--;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no unitialized directions left.");
        }
    }
}
