using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MazeCell currentCell;

    public void SetLocation(MazeCell cell)
    {
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        //currentDirection = MazeDirection.North;
    }

    private void Move(MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction); // FIXME
        if(edge is MazePassage)
        {
            SetLocation(edge.otherCell);
        }
    }

    public float rotationAngleSpeed;

    private void TurnLeft()
    {
        transform.Rotate(Vector3.up, -rotationAngleSpeed);
    }

    private void TurnRight()
    {
        transform.Rotate(Vector3.up, rotationAngleSpeed);
    }

    private MazeDirection currentDirection = MazeDirection.North;
    private void Look(MazeDirection direction)
    {
        transform.localRotation = direction.ToRotation();
        currentDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Move(currentDirection);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Move(currentDirection.GetNextClockwise());
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Move(currentDirection.GetOpposite());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Move(currentDirection.GetNextCounterclockwise());
        }
        else if(Input.GetKey(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.Q))
        {
            Look(currentDirection.GetNextCounterclockwise());
        }
        else if(Input.GetKey(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.E))
        {
            Look(currentDirection.GetNextClockwise());
        }
    }
}