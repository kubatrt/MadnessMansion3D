using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;

    private Maze mazeInstance;


    // Start is called before the first frame update
    void Start()
    {
        BeginGame();

        Debug.Log("Random: " + MazeDirections.RandomValue);


       Maze maze = mazeInstance.GetComponent<Maze>();
        if (maze) Debug.Log("Maze found!");
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void BeginGame()
    {
        Debug.Log("Begin");

        mazeInstance = Instantiate<Maze>(mazePrefab);
        StartCoroutine(mazeInstance.Generate());
    }

    private void RestartGame()
    {
        Debug.Log("Restart");
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
