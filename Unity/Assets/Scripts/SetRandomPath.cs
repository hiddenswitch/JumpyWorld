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
		public float height;
		public int minLength;
		public int maxLength;
		public Source<Vector3[]> pathSource;
		public List<Vector3> path;

		// Use this for initialization
		public void Start ()
		{
			pathSource = pathSource ?? this.gameObject.GetComponent<Source<Vector3[]>> ();
			var oldSeed = Random.seed;
			Random.seed = seed;

			path = CreatePath (bounds, height, minLength, maxLength);

			if (pathSource != null) {
				pathSource.value = path.ToArray ();
			}

			this.gameObject.BroadcastMessage ("DelayedStart");

			Random.seed = oldSeed;
		}

		/// <summary>
		/// Creates a path of length between lengthLB and lengthUB in the 
		/// bounds of the rectangle at a y height of height
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="rectangle">Rectangle.</param>
		/// <param name="height">Height.</param>
		/// <param name="lengthLB">Length L.</param>
		/// <param name="lengthUB">Length U.</param>
		public static List<Vector3> CreatePath (Rect rectangle, float height, int lengthLB, int lengthUB)
		{
			int stepsLeft = lengthUB;
			Vector3 start;
			Vector3 currentPt;

			List<Vector3> generatedPath = new List<Vector3> ();

			if (lengthUB < lengthLB + 5) {
				Debug.LogError ("maxLength needs to be at least 5 greater than minLength");
			}
			
			if (lengthLB < 5) {
				Debug.LogError ("minLength needs to be at least 5");
			}
			
			// set a random starting point
			int startX = Random.Range ((int)rectangle.xMin, (int)rectangle.xMax);
			int startZ = Random.Range ((int)rectangle.yMin, (int)rectangle.yMax);
			start = new Vector3 (startX, height, startZ);

			generatedPath.Add (start);
			currentPt = start;

			// pick each successive point in the path
			Vector3 nextPt;
			while (stepsLeft > lengthUB - lengthLB) {
				nextPt = PickNextPoint (stepsLeft, generatedPath, lengthUB, rectangle);
				generatedPath.Add (nextPt);
				stepsLeft -= XZEuclideanDistance (currentPt, nextPt);
				currentPt = nextPt;
			}
			
			// close the path
			if (currentPt != start) {
				Vector3 lastDirection = (generatedPath [generatedPath.Count - 2] - currentPt).normalized;
				if ((lastDirection == Vector3.left || lastDirection == Vector3.right) && currentPt.z != start.z) {
					generatedPath.Add (new Vector3 (currentPt.x, height, start.z));
				} else if ((lastDirection == Vector3.forward || lastDirection == Vector3.back) && currentPt.x != start.x) {
					generatedPath.Add (new Vector3 (start.x, height, currentPt.z));
				}
				if (generatedPath [generatedPath.Count - 1] != start) {
					generatedPath.Add (start);
				}
			}

			return generatedPath;
		}

		/// <summary>
		/// Helper function that picks the next point for generatedPath
		/// </summary>
		/// <returns>The next point.</returns>
		/// <param name="stepsLeft">Steps left.</param>
		/// <param name="generatedPath">Generated path.</param>
		/// <param name="lengthUB">Length U.</param>
		/// <param name="rectangle">Rectangle.</param>
		static Vector3 PickNextPoint (int stepsLeft, List<Vector3> generatedPath, int lengthUB, Rect rectangle)
		{
			Vector3 currentPt = generatedPath [generatedPath.Count - 1];
			Vector3 lastDirection = Vector3.zero;
			if (generatedPath.Count > 1) {
				lastDirection = (generatedPath [generatedPath.Count - 2] - currentPt).normalized;
			} 
			List<Vector3> allPossibleNext = GetAllPossibleNext (currentPt, stepsLeft, generatedPath [0], lastDirection, lengthUB, rectangle);
			Vector3 randomNext = allPossibleNext [Random.Range (0, allPossibleNext.Count - 1)];
			return randomNext;
		}


		/// <summary>
		/// Helper function that calculates all possible next points starting at point,
		/// ensuring that the next point makes it possible to reach start in stepsLeft
		/// </summary>
		/// <returns>All poissible next points.</returns>
		/// <param name="point">Point.</param>
		/// <param name="stepsLeft">Steps left.</param>
		/// <param name="start">Start.</param>
		/// <param name="lastDirection">Last direction.</param>
		/// <param name="lengthUB">Length U.</param>
		/// <param name="bounds">Bounds.</param>
		static List<Vector3> GetAllPossibleNext (Vector3 point, int stepsLeft, Vector3 start, Vector3 lastDirection, int lengthUB, Rect bounds)
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
							!InBounds (potentialStep, bounds) ||
							(point - potentialStep).magnitude > lengthUB / 4) {
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

		/// <summary>
		/// Checks if point is in bounds
		/// </summary>
		/// <returns><c>true</c>, if point in bounds, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		/// <param name="bounds">Bounds.</param>
		static bool InBounds (Vector3 point, Rect bounds)
		{
			return (bounds.xMin <= point.x) && (point.x <= bounds.xMax) && (bounds.yMin <= point.z) && (point.z <= bounds.yMax);
		}

		/// <summary>
		/// Calculates the XZ Euclidean distance between a and b
		/// </summary>
		/// <returns>The euclidean distance.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		static int XZEuclideanDistance (Vector3 a, Vector3 b)
		{
			int xDist = (int)Mathf.Abs (a.x - b.x);
			int zDist = (int)Mathf.Abs (a.z - b.z);
			return xDist + zDist;
		}
	}
}
