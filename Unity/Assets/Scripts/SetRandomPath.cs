using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class SetRandomPath : MonoBehaviour
	{

		//public Vector3[] obstacles;
		public int seed;
		public Rect bounds;
		public int height;
		public int minLength;
		public int maxLength;
		public Source<Vector3[]> pathSource;

		private List<Vector3> path = new List<Vector3> ();
		private Vector3 start;
		private int stepsLeft;
		private Vector3 currentPt;
		private Vector3 lastDirection = Vector3.zero;

		// Use this for initialization
		void Start ()
		{
			pathSource = this.gameObject.GetComponent<Source<Vector3[]>> ();
			var oldSeed = Random.seed;
			Random.seed = seed;

			stepsLeft = maxLength;

			if (maxLength < minLength + 5) {
				Debug.LogError ("maxLength needs to be at least  greater than minLength");
			}

			// set a random starting point
			int startX = Random.Range ((int)bounds.xMin, (int)bounds.xMax);
			int startZ = Random.Range ((int)bounds.yMin, (int)bounds.yMax);
			start = new Vector3 (startX, height, startZ);
			currentPt = start;
			path.Add (start);

			// pick each successive point in the path
			while (stepsLeft > maxLength - minLength) {
				stepsLeft -= PickNextPoint (stepsLeft);
			}

			// close the path
			if (currentPt != start) {
				if (lastDirection == Vector3.left || lastDirection == Vector3.right) {
					path.Add (new Vector3 (currentPt.x, height, start.z));
				} else {
					path.Add (new Vector3 (start.x, height, currentPt.z));
				}
				path.Add (start);
			}

			Debug.Log (pathSource);
			pathSource.value = path.ToArray ();

			this.gameObject.BroadcastMessage ("DelayedStart");
			//Debug.Log (pathSource.value);

			Random.seed = oldSeed;
		}

		// drawing the path
		void OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
			if (path == null 
				|| path.Count == 0) {
				return;
			}

			for (var i = 0; i < path.Count - 1; i++) {
				Gizmos.DrawLine (path [i], path [i + 1]);
			}
		}

		// picking the next point where stepsLeft is the number of steps left
		int PickNextPoint (int stepsLeft)
		{
			List<Vector3> allPossibleNext = GetAllPossibleNext (currentPt, stepsLeft);
			Vector3 randomNext = allPossibleNext [Random.Range (0, allPossibleNext.Count - 1)];
			var distanceTraveled = XZEuclideanDistance (currentPt, randomNext);
			lastDirection = (randomNext - currentPt).normalized;
			path.Add (randomNext);
			currentPt = randomNext;
			return distanceTraveled;
		}


		// gets all points that point can got to and back to start ing stepLeft steps
		List<Vector3> GetAllPossibleNext (Vector3 point, int stepsLeft)
		{
			Vector3[] directions = {
				Vector3.left,
				Vector3.right,
				Vector3.forward,
				Vector3.back
			};
			List<Vector3> possibleNexts = new List<Vector3> ();
			foreach (var direction in directions) {
				if (direction != lastDirection && direction != lastDirection * -1) {
					int stepSize = 1;
					bool noMoreSteps = false;
					while (!noMoreSteps) {
						Vector3 potentialStep = direction * stepSize + point;
						if (XZEuclideanDistance (potentialStep, start) > stepsLeft || 
							!inBounds (potentialStep) ||
							(point - potentialStep).magnitude > maxLength / 4) {
							noMoreSteps = true;
						} else {
							stepSize += 1;
							possibleNexts.Add (potentialStep);
						}
					}
				}
			}
			return possibleNexts;
		}

		// check if the point is in bounds
		bool inBounds (Vector3 point)
		{
			return (bounds.xMin <= point.x) && (point.x <= bounds.xMax) && (bounds.yMin <= point.z) && (point.z <= bounds.yMax);
		}

		// get the Euclidean distance in the XZ plane from a to b
		int XZEuclideanDistance (Vector3 a, Vector3 b)
		{
			int xDist = (int)Mathf.Abs (a.x - b.x);
			int zDist = (int)Mathf.Abs (a.z - b.z);
			return xDist + zDist;
		}
	}
}
