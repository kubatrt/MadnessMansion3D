using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MazeGenerationMethod
{
    Last,
    Random,
    Middle,
    First
}

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    public MazeGenerationMethod mazeGenerationMethod = MazeGenerationMethod.Last;
    public Player playerPrefab;

    private Player playerInstance;
    private Maze mazeInstance;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginGame());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private IEnumerator BeginGame()
    {
        Debug.Log("BeginGame");
        mazeInstance = Instantiate<Maze>(mazePrefab);
        mazeInstance.SetMazeGenerationMethod(MazeGenerationMethod.Last);
        yield return StartCoroutine(mazeInstance.Generate());
        Debug.Log("Player spawn");
        playerInstance = Instantiate(playerPrefab);
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if(playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }
}
