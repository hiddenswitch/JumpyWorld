using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class PathPlacer : Generator {
		public int seed;
		public Floor room;
		public float height = 1f;
		public float step = 1f;
		// for now assume numMosters = 1
		public int numMosters = 1;
		public GameObject pathablePrefab;

		public override void Generate (int seed)
		{
			base.Generate (seed);
		}

		public override void Draw (TileDrawer tileDrawer=null, TilePool tilePool=null)
		{
			Rect[] miniRects = partitionRect (room.size, numMosters);
			List<Vector3> path;
			for (var i = 0; i < numMosters; i++) {
				int[] minAndMax = generateMinAndMaxForRect (miniRects[i]);
				path = JumpyWorld.SetRandomPath.createPath (miniRects[i], height, minAndMax[0], minAndMax[1]);
				// draw the path
				for (var j = 0; j < path.Count; j++) {
					if (j == path.Count -1) {
						DrawLine(tileDrawer, tilePool, path[j], path[0]);
					} else {
						DrawLine(tileDrawer, tilePool, path[j], path[j+1]);
					}
				}

				//Vector3 monsterStart =  path[0];
				var ghost = GameObject.Instantiate(pathablePrefab, Vector3.zero, Quaternion.identity) as GameObject;
				var movesAlongPath = ghost.GetComponent<MovesAlongPath>();
				movesAlongPath.path = path.ToArray ();
				movesAlongPath.shouldTeleportToStartOfPath = true;

				// for now hard codes
				movesAlongPath.offset = .5f;
				ghost.BroadcastMessage ("DelayedStart");
			}
		}

		void DrawLine(TileDrawer tileDrawer, TilePool tilePool, Vector3 start, Vector3 end) {
			List<Vector3> pointsToDraw = JumpyWorld.Hallway.BresenhamFilledPath (start, end);
			for(var i = 0; i < pointsToDraw.Count; i++) {
				tileDrawer.DrawTerrain (prefab: tilePool.path, at: pointsToDraw[i]);
			}
		}

		Rect[] partitionRect(Rect rect, int numPartitions) {
			/*Rect[] partitions = new Rect[numPartitions];
			// Later: do something cute here that actually partitions the room....
			for (var i = 0; i < numPartitions; i++) {
				// fix
				partitions[i] = rect;
			}*/
			Rect[] partitions = Split (rect, new Vector2 (3, 3), numPartitions).ToArray();
			return partitions;
		}


		// generates the min and max lengths for a path that will be in the bounds of rect
		int[] generateMinAndMaxForRect(Rect rect) {
			int[] minAndMax = new int[2];
			minAndMax [0] = (int) Mathf.Max(5, (rect.width + rect.height));
			minAndMax [1] = (int) Mathf.Max (5 + minAndMax[0], rect.width * 3 + rect.height * 3);
			return minAndMax;
		}

		// the two split functions are copied and modified from the web
		List<Rect> SplitOnce(this Rect aSource, Vector2 aMinSize)
		{
			float aspect = aMinSize.x/aMinSize.y;
			if (aSource.width > aSource.height*aspect)
			{
				if (aSource.width > aMinSize.x*2)
				{
					float range = (aSource.width-aMinSize.x*2);
					float split = Random.Range(0,range) + aMinSize.x;
					Rect R1 = aSource;
					Rect R2 = aSource;
					R1.xMax -= split;
					R2.xMin = R1.xMax;
					var result = new List<Rect>();
					result.Add(R1);
					result.Add(R2);
					return result;
				}
				else
					return null;
			}
			else
			{
				if (aSource.height > aMinSize.y*2)
				{
					float range = (aSource.height-aMinSize.y*2);
					float split = Random.Range(0,range) + aMinSize.y;
					Rect R1 = aSource;
					Rect R2 = aSource;
					R1.yMax -= split;
					R2.yMin = R1.yMax;
					var result = new List<Rect>();
					result.Add(R1);
					result.Add(R2);
					return result;
				}
				else
					return null;
			}
		}
			
		List<Rect> Split(this Rect aSource, Vector2 aMinSize, int numSplits)
		{
			var result = new List<Rect>();
			if (numSplits == 1){
				result.Add (aSource);
				return result;
			}
			var tmp = SplitOnce(aSource,aMinSize);
			if (tmp != null)
			{
				foreach(Rect R in tmp)
				{
					int nextNumSplit = (int) Mathf.Ceil (Mathf.Log(numSplits)/Mathf.Log (2));
					result.AddRange(Split(R,aMinSize, nextNumSplit));
				}
			}
			else 
				result.Add(aSource);
			return result;
		}
	}
}