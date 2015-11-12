using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class Generator : Placeable
	{
		public TileDrawer tileDrawer;
		public TilePool tilePool;
		public int seed;
		public Anchor[] anchors;
		public bool generateOnStart = false;

		void Start ()
		{
			if (generateOnStart) {
				Build ();
			}
		}

		public void Build ()
		{
			tileDrawer = tileDrawer ?? GetComponent<TileDrawer> () ?? TileDrawer.instance;
			tilePool = tilePool ?? GetComponent<TilePool> () ?? TilePool.instance;

			var oldSeed = Random.seed;
			Random.seed = seed;

			Generate (seed: seed);
			Draw (tileDrawer: tileDrawer, tilePool: tilePool);
			for (int i = 0; i < anchors.Length; i++) {
				anchors [i].generator = this;
			}
			Random.seed = oldSeed;
		}

		public virtual void Generate (int seed)
		{

		}

		public virtual void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{

		}
		
		public virtual void OnDrawGizmos ()
		{
			Gizmos.color = Color.blue;
			foreach (var anchor in anchors) {
				Gizmos.DrawWireCube (anchor.position, new Vector3 (1, 1, 1));
			}
		}

		public static List<Vector3> Perturb (List<Vector3> points, float turbulence=2.0f)
		{
			Vector3 newPoint;
			var newPoints = new List<Vector3> (points.Count);
			newPoints.Add (points [0]);
			foreach (var point in points.GetRange (1,points.Count-2)) {
				var displacement = turbulence * Random.insideUnitSphere;
				displacement.y = 0;
				newPoint = (displacement + point);
				newPoint.x = Mathf.Round (newPoint.x);
				newPoint.z = Mathf.Round (newPoint.z);
				newPoints.Add (newPoint);
			}
			newPoints.Add (points [points.Count - 1]);
			return newPoints;
		}

		public static List<Vector3> MakePathable (List<Vector3> points)
		{
			List<Vector3> path = new List<Vector3> ();
			for (int i=1; i < points.Count; i++) {
				List<Vector3> PathPostions = Hallway.BresenhamFilledPath (points [i - 1], points [i]);
				path.AddRange (PathPostions);
			}
			return path;
		}


	}
}
