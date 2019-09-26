using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage
{
    public Transform hinge;
    public Transform frame;

    private static Quaternion normalRotation = Quaternion.Euler(0f, -90f, 0f);
    private static Quaternion mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

    private bool isMirrored;

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
            isMirrored = true;
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

    public override void OnPlayerEntered()
    {
        OtherSideOfDoor.hinge.localRotation = hinge.localRotation =
            isMirrored ? mirroredRotation : normalRotation;
        OtherSideOfDoor.cell.room.Show();
    }

    public override void OnPlayerExited()
    {
        OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
        //OtherSideOfDoor.cell.room.Hide();
    }
}
