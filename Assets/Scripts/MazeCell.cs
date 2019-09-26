using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Vector2Int coordinates;
    public MazeRoom room;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

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

    public void Visitted()
    {
        //int mask = gameObject.layer;
        //mask |= (1 << LayerMask.NameToLayer("Visitted"));
        gameObject.layer = LayerMask.NameToLayer("Visitted");
        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Visitted");
        }
    }

    public void OnPlayerEntered()
    {
        room.MarkVisitted();
        room.Show();
        for (int i = 0; i < edges.Length; ++i)
        {
            edges[i].OnPlayerEntered();
        }
    }

    public void OnPlayerExited()
    {
        //room.Hide();
        for (int i = 0; i < edges.Length; ++i)
        {
            edges[i].OnPlayerExited();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
