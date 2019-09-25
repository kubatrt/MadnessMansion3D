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
        Debug.Log("BeginGame");
        mazeInstance = Instantiate<Maze>(mazePrefab);
        StartCoroutine(mazeInstance.Generate());
    }

    private void RestartGame()
    {
        Debug.Log("RestartGame");
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
