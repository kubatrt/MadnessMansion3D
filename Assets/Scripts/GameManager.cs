using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    public GameObject mazeObject;

    private Maze mazeInstance;


    // Start is called before the first frame update
    void Start()
    {
        BeginGame();

        if (mazeObject == null) return;

        Maze maze = mazeObject.GetComponent<Maze>();
        if (maze) Debug.Log("Maze found!");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space pressed");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void BeginGame()
    {
        Debug.Log("Begin");

        mazeInstance = Instantiate<Maze>(mazePrefab);
    }

    private void RestartGame()
    {
        Debug.Log("Restart");
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
