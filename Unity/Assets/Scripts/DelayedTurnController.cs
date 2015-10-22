using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayedTurnController : MonoBehaviour {
    public float cellSize;
    public float enterCellThreshold;
    public float exitCellThreshold;

    Vector3 currentDirection;
    Queue<Vector3> directionQueue; //should be length 1 at most;

    [Header("Runtime")]
    public bool isAlive = true;


    // Use this for initialization
    void Start () {
        currentDirection = new Vector3(0, 0, 0);
        directionQueue = new Queue<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        if (canTurnSideways() && directionQueue.Count > 0)
        {
            currentDirection = (directionQueue.Dequeue());
            BroadcastMessage("TurnDirection", currentDirection);
        }
    }

    bool canTurnSideways()
    {
        //assumes all cells are gridded, and (0,0) is the valid center;
        float xOffset = transform.position.x - Mathf.Round(transform.position.x / cellSize) * cellSize;
        float yOffset = transform.position.z - Mathf.Round(transform.position.z / cellSize) * cellSize;
        return (xOffset > enterCellThreshold && yOffset > enterCellThreshold && xOffset < exitCellThreshold && yOffset < exitCellThreshold);
    }


    void handleTurnAngle(Vector3 targetRotationDegrees)
    {
        Debug.Log(Mathf.Abs(currentDirection.y - targetRotationDegrees.y));
        if (Mathf.Abs(currentDirection.y - targetRotationDegrees.y) == 90f || Mathf.Abs(currentDirection.y - targetRotationDegrees.y) == 270f)
        {
            directionQueue.Clear();
            directionQueue.Enqueue(targetRotationDegrees);
        }
        else
        {
            directionQueue.Clear();
            BroadcastMessage("TurnDirection", targetRotationDegrees);
            currentDirection = targetRotationDegrees;
        }
    }


    void HandleOnGameOver()
    {
        enabled = false;
    }

    void SwipeNorth()
    {
        if (!isAlive)
        {
            return;
        }
        handleTurnAngle(new Vector3(0f, 0f, 0f));
    }

    void SwipeWest()
    {
        if (!isAlive)
        {
            return;
        }
        handleTurnAngle(new Vector3(0f, -90f, 0f));
    }

    void SwipeEast()
    {
        if (!isAlive)
        {
            return;
        }
        handleTurnAngle(new Vector3(0f, 90f, 0f));
    }

    void SwipeSouth()
    {
        if (!isAlive)
        {
            return;
        }
        handleTurnAngle(new Vector3(0f, 180f, 0f));
    }
}
