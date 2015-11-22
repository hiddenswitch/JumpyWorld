using UnityEngine;
using System.Collections;
// is this okay?
using System.Collections.Generic;

public class SetRandomPath : MonoBehaviour {

	//public Vector3[] obstacles;
	public Rect bounds;
	public int height;
	public int minLength;
	public int maxLength;

	private List<Vector3> path = new List<Vector3>();
	private int stepsLeft;
	private Vector3 currentPt;

	// Use this for initialization
	void Start () {
		stepsLeft = maxLength;
		int startX = Random.Range ((int)bounds.xMin, (int)bounds.xMax);
		int startZ = Random.Range ((int)bounds.yMin, (int)bounds.yMax);
		var start = new Vector3 (startX, height, startZ);
		currentPt = start;
		int i = 10;
		while (stepsLeft > maxLength - minLength && i != 0) {
			Debug.Log (currentPt);
			stepsLeft -= PickNextPoint(currentPt, stepsLeft, start);
			i-=1;
		}
		Debug.Log (stepsLeft);
		Debug.Log (path.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*void OnDrawGizmos () {
		if (obstacles == null) {
			return;
		}
		if (obstacles.Length == 0) {
			return;
		}
		foreach (var pt in obstacles) {
			Gizmos.DrawCube(pt, new Vector3(1,1,1));
		}
	}*/

	int PickNextPoint (Vector3 point, int stepsLeft, Vector3 end) {
		List<Vector3> allPossibleNext = GetAllPossibleNext (point, stepsLeft, end);
		Vector3 randomNext = allPossibleNext [Random.Range (0, allPossibleNext.Count)];
		Debug.Log (randomNext);
		path.Add (randomNext);
		currentPt = randomNext;
		return XZEuclideanDistance (point, randomNext);
	}
	
	List<Vector3> GetAllPossibleNext(Vector3 point, int stepsLeft, Vector3 end) {
		Vector3[] directions = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
		List<Vector3> possibleNexts = new List<Vector3>();
		foreach (var direction in directions) {
			int stepSize = 1;
			bool noMoreSteps = false;
			while (!noMoreSteps) {
				Vector3 potentialStep = direction * stepSize;
				if (XZEuclideanDistance(potentialStep, end) > stepsLeft || inBounds (potentialStep)) {
					noMoreSteps = true;
				} else {
					stepSize += 1;
					possibleNexts.Add(potentialStep);
				}
			}
		}
		return possibleNexts;
	}

	bool inBounds(Vector3 point) {
		return (bounds.xMin <= point.x) && (point.x <= bounds.xMax) && (bounds.yMin <= point.z) && (point.z <= bounds.yMax);
	}

	int XZEuclideanDistance(Vector3 a, Vector3 b) {
		int xDist = (int) Mathf.Abs (a.x - b.x);
		int zDist = (int) Mathf.Abs (a.z - b.z);
		return xDist + zDist;
	}
}
