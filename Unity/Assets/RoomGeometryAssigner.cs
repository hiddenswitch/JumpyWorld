using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RoomGeometryAssigner : MonoBehaviour {

		public TileDrawer tileDrawer;
		public TilePool tilePool;
		public int seed;
		public Anchor[] anchors;
		public bool generateOnStart = false;
		public Floor room;
		public float height = 1f;
		public float step = 1f;

		// Use this for initialization
		void Start () {

			// attach an obstacle placer to this gameObject, and assign random values to it
			var obstaclePlacer = gameObject.AddComponent<ObstaclePlacerForRoom>();

			// first transfer over default values assigned to this object
			obstaclePlacer.tileDrawer = tileDrawer;
			obstaclePlacer.tilePool = tilePool;
			obstaclePlacer.seed = seed;
			obstaclePlacer.anchors = anchors;
			obstaclePlacer.generateOnStart = generateOnStart;
			obstaclePlacer.room = room;
			obstaclePlacer.height = height;
			obstaclePlacer.step = step;


			int randomMazeSeed = (int)System.DateTime.Now.Ticks;
			Random.seed = randomMazeSeed;

			// then customize the geometry
			if (Random.value > 0.5f) {
				obstaclePlacer.obstaclePattern = obstaclePlacer.obstaclePattern | ObstaclePattern.CenterCross;
			}
			obstaclePlacer.crossRadius = Random.Range(1,1+Mathf.CeilToInt((room.size.width/2.0f)));
			obstaclePlacer.crossCenterClearingRadius = Random.Range(0,obstaclePlacer.crossRadius);

			// add diagonals
			if (Random.value > 0.5f) {
				obstaclePlacer.obstaclePattern = obstaclePlacer.obstaclePattern | ObstaclePattern.Diagonals;
			}
			obstaclePlacer.diagonalsRadius = Random.Range(1,1+Mathf.CeilToInt((room.size.width/2.0f) * 1.412f));
			obstaclePlacer.diagonalCenterClearingRadius = Random.Range(0,obstaclePlacer.diagonalsRadius);

			// add corner edges
			if (Random.value > 0.5f) {
				obstaclePlacer.obstaclePattern = obstaclePlacer.obstaclePattern | ObstaclePattern.CornerEdges;
			}
			obstaclePlacer.cornerExtent = Random.Range(1,Mathf.CeilToInt((room.size.width/2.0f)-1));

			// add corner edges
			if (Random.value > 0.5f) {
				obstaclePlacer.obstaclePattern = obstaclePlacer.obstaclePattern | ObstaclePattern.Stripes;
			}
			obstaclePlacer.stripeSpacing = Random.Range(2,2+Mathf.CeilToInt(room.size.width/4.0f));
			obstaclePlacer.stripeDistanceFromBorders = Random.Range(1,Mathf.CeilToInt(room.size.width/2.0f));
			if (Random.value > 5.0f) {
				obstaclePlacer.stripeAxis = Directions.East;
			}

//			if ((obstaclePlacer.obstaclePattern & ObstaclePattern.None) == ObstaclePattern.None) {
//				
//			}
		}
	}
}