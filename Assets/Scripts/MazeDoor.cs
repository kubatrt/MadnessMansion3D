using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage
{
    public Transform hinge;
    public Transform frame;

    private MazeDoor OtherSideOfDoor
    {
        get
        {
            return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
        }
    }

    public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        base.Initialize(cell, otherCell, direction);
        if(OtherSideOfDoor != null)
        {
            //hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 p = hinge.localPosition;
            p.x = -p.x;
            hinge.localPosition = p;
            // SUPPRESS WARNING
            foreach (Transform t in hinge.transform.GetComponentsInChildren<Transform>())
            {
                t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
            }
        }

        for (int i = 0; i < frame.transform.childCount; ++i)
        {
            Transform child = frame.transform.GetChild(i);
            if(child != hinge)
            {
                child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
            }
        }
    }
}
