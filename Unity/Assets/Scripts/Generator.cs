﻿using UnityEngine;
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

		void Start ()
		{
			tileDrawer = tileDrawer ?? GetComponent<TileDrawer> () ?? TileDrawer.instance;
			tilePool = tilePool ?? GetComponent<TilePool> () ?? TilePool.instance;

			var oldSeed = Random.seed;
			Random.seed = seed;

			Generate (seed: seed);
			Draw (tileDrawer: tileDrawer, tilePool: tilePool);

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
            Vector3 newpoint;
			var newPoints = new List<Vector3> (points.Count);
            newPoints.Add (points[0]);
			foreach (var point in points.GetRange (1,points.Count-2)) {
				var displacement = turbulence * Random.insideUnitSphere;
				displacement.y = 0;
                newpoint=(displacement + point);
                newpoint.x=Mathf.Round (newpoint.x);
                newpoint.z=Mathf.Round(newpoint.z);
				newPoints.Add(newpoint);
			}
            newPoints.Add (points[points.Count-1]);
			return newPoints;
//			for (int i =0; i< points.Count; i++) {
//				int index = 3 * Random.Range (1, (points.Count - 2) / 3);
//				Vector3 old = points [index];
//				points [index] = old + new Vector3 (Random.Range (0, 3), 0, Random.Range (0, 3));
//			}
		}

		public static List<Vector3> MakePathable (List<Vector3> points)
		{
//			points.Sort(delegate(Vector3 pt1, Vector3 pt2) {
//				return ;
//			});
			List<Vector3> path = new List<Vector3> ();
			for (int i=1; i < points.Count; i++) {
//				if (points [i - 1] != points [i]) {
//					path.Add (points [i - 1]);
//					if (Vector3.Distance (points [i - 1], points [i]) < 2) {
//						if (points [i - 1].z != points [i].z || points [i - 1].x != points [i].x) {
//							path.Add (FillPath (points [i - 1], points [i]));
//						}
//					} else {
						List<Vector3> PathPostions = Hallway.BresenhamFilledPath (points [i - 1], points [i]);
						path.AddRange (PathPostions);
//					}
//				}
			}
			return path;
		}


	}
}
