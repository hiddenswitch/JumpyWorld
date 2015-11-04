using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class Hallway : MonoBehaviour
	{
		public TerrainDrawer terrainDrawer;
		public TerrainPool terrainPool;
		[Header("Optional")]
		public Vector3
			startPoint;
		public Vector3 endPoint;
		public int seed;
		[Header("Runtime")]
		public List<Vector3>
			pathBackBone;
		public List<Vector3> pathPositions = new List<Vector3> ();


		// Use this for initialization
		void Start ()
		{
			var oldRandom = Random.seed;
			Random.seed = seed;

			terrainDrawer = terrainDrawer ?? GetComponent<TerrainDrawer> ();
			terrainPool = terrainPool ?? GetComponent<TerrainPool> ();

			// Create the path
			pathPositions = BresenhamFilledPath (from: startPoint, to: endPoint);

			// Draw the terrain
			for (int i = 0; i < pathPositions.Count; i++) {
				terrainDrawer.DrawTerrain (tile: terrainPool.defaultGround, at: pathPositions [i]);
			}

			Random.seed = oldRandom;
		}

		List<Vector3> BresenhamFilledPath (Vector3 from=default(Vector3), Vector3 to=default(Vector3))
		{
			var path = new List<Vector3> ();
			// Change the origin of the bresenham to be at the start point
			var bresenham = new UnifyCommunity.Bresenham3D (Vector3.zero, to - from);
			// Detect two patterns and fill in a corner:
			// #        #
			//  #  or  #
			// Fill in any of the other corners when that pattern is detected
			Vector3[] lastTwoPoints = new Vector3[2];
			// Use a toggle value to alternate which corner gets filled
			bool toggle = false;

			foreach (var point in bresenham) {
				lastTwoPoints [0] = lastTwoPoints [1];
				lastTwoPoints [1] = point;
				// Detect if a diagonal pattern occurred
				if (lastTwoPoints [0].z != lastTwoPoints [1].z && lastTwoPoints [0].x != lastTwoPoints [1].x) {
					// Fill in a corner point
					var cornerPoint = Vector3.zero;
					if (toggle) {
						cornerPoint.x = Mathf.Min (lastTwoPoints [0].x, lastTwoPoints [1].x);
						cornerPoint.z = Mathf.Max (lastTwoPoints [0].z, lastTwoPoints [1].z);
					} else {
						cornerPoint.x = Mathf.Max (lastTwoPoints [0].x, lastTwoPoints [1].x);
						cornerPoint.z = Mathf.Min (lastTwoPoints [0].z, lastTwoPoints [1].z);
					}
					toggle = !toggle;
					path.Add (cornerPoint + from);
				}
				path.Add (point + from);
			}

			return path;
		}
	}
}
