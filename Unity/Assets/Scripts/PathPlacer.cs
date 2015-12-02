using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class PathPlacer : Generator
	{
		public Floor floor;
		public float height = 1f;
		public float step = 1f;
		public int numMonsters = 1;
		public GameObject pathablePrefab;
		private Vector2 minSizeOfSplit = new Vector2 (3, 3);

		public override void Generate (int seed)
		{
			base.Generate (seed);
		}

		public override void Draw (TileDrawer tileDrawer=null, TilePool tilePool=null)
		{
			bool enabled = true;

			if (Mathf.FloorToInt (floor.size.width / minSizeOfSplit.x) * Mathf.FloorToInt (floor.size.height / minSizeOfSplit.y) < numMonsters) {
				enabled = false;
				Debug.LogError ("You have too many monsters on the floor");
			}

			if (enabled) {
				Rect[] miniRects = PartitionRect (floor.size, numMonsters);
				List<Vector3> path;
				for (var i = 0; i < numMonsters; i++) {
					int[] minAndMax = GenerateMinAndMaxForRect (miniRects [i]);
					path = JumpyWorld.SetRandomPath.CreatePath (miniRects [i], height, minAndMax [0], minAndMax [1]);
					// draw the path
					for (var j = 0; j < path.Count; j++) {
						if (j == path.Count - 1) {
							DrawLine (tileDrawer, tilePool, path [j], path [0]);
						} else {
							DrawLine (tileDrawer, tilePool, path [j], path [j + 1]);
						}
					}

					// instantiate ghost
					var ghost = GameObject.Instantiate (pathablePrefab, Vector3.zero, Quaternion.identity) as GameObject;
					var movesAlongPath = ghost.GetComponent<MovesAlongPath> ();
					movesAlongPath.path = path.ToArray ();
					movesAlongPath.shouldTeleportToStartOfPath = true;
					// hard coded to lift ghost to play level
					movesAlongPath.offset = .5f;
					ghost.BroadcastMessage ("DelayedStart");
				}
			}
		}

		/// <summary>
		/// Draws terrain from start to finish using the tileDrawer
		/// </summary>
		/// <param name="tileDrawer">Tile drawer.</param>
		/// <param name="tilePool">Tile pool.</param>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		void DrawLine (TileDrawer tileDrawer, TilePool tilePool, Vector3 start, Vector3 end)
		{
			List<Vector3> pointsToDraw = JumpyWorld.Hallway.BresenhamFilledPath (start, end);
			for (var i = 0; i < pointsToDraw.Count; i++) {
				tileDrawer.DrawTerrain (prefab: tilePool.path, at: pointsToDraw [i]);
			}
		}

		Rect[] PartitionRect (Rect rect, int numPartitions)
		{
			Rect[] partitions = Split (rect, minSizeOfSplit, numPartitions).ToArray ();
			return partitions;
		}

		/// <summary>
		/// Generates the minimum and max lengths for a path that wil be in the bounds of rect.
		/// </summary>
		/// <returns>The minimum and max for rect in an Arrray.</returns>
		/// <param name="rect">Rect.</param>
		int[] GenerateMinAndMaxForRect (Rect rect)
		{
			int[] minAndMax = new int[2];
			minAndMax [0] = (int)Mathf.Max (5, (rect.width + rect.height));
			minAndMax [1] = (int)Mathf.Max (5 + minAndMax [0], rect.width * 3 + rect.height * 3);
			return minAndMax;
		}

		// the two split functions are copied and modified from the web
		/// <summary>
		/// Splits a rectangle aSource once into two rectangles that are at 
		/// least aMinSize in size
		/// </summary>
		/// <returns>The once.</returns>
		/// <param name="aSource">A source.</param>
		/// <param name="aMinSize">A minimum size.</param>
		static List<Rect> SplitOnce (Rect aSource, Vector2 aMinSize)
		{
			float aspect = aMinSize.x / aMinSize.y;
			if (aSource.width > aSource.height * aspect) {
				if (aSource.width > aMinSize.x * 2) {
					float range = (aSource.width - aMinSize.x * 2);
					float split = Random.Range (0, range) + aMinSize.x;
					Rect R1 = aSource;
					Rect R2 = aSource;
					R1.xMax -= split;
					R2.xMin = R1.xMax;
					var result = new List<Rect> ();
					result.Add (R1);
					result.Add (R2);
					return result;
				} else
					return null;
			} else {
				if (aSource.height > aMinSize.y * 2) {
					float range = (aSource.height - aMinSize.y * 2);
					float split = Random.Range (0, range) + aMinSize.y;
					Rect R1 = aSource;
					Rect R2 = aSource;
					R1.yMax -= split;
					R2.yMin = R1.yMax;
					var result = new List<Rect> ();
					result.Add (R1);
					result.Add (R2);
					return result;
				} else
					return null;
			}
		}

		/// <summary>
		/// Splits aSource into numSplits rectangles, each at least aMinSize in size
		/// </summary>
		/// <param name="aSource">A source.</param>
		/// <param name="aMinSize">A minimum size.</param>
		/// <param name="numSplits">Number splits.</param>
		static List<Rect> Split (Rect aSource, Vector2 aMinSize, int numSplits)
		{
			var result = new List<Rect> ();
			if (numSplits == 1) {
				result.Add (aSource);
				return result;
			}
			var tmp = SplitOnce (aSource, aMinSize);
			if (tmp != null) {
				foreach (Rect R in tmp) {
					int nextNumSplit = (int)Mathf.Ceil (Mathf.Log (numSplits) / Mathf.Log (2));
					result.AddRange (Split (R, aMinSize, nextNumSplit));
				}
			} else 
				result.Add (aSource);
			return result;
		}
	}
}