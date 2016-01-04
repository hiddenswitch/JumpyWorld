using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class ObstacleGenerator : Generator
	{
		public struct Obstacle
		{
			public Vector3 position;
			public GameObject tile;
		}

		public Floor room;
		public float height = 1f;
		[Header ("Runtime")]
		public HashSet<Obstacle>
			obstacles = new HashSet<Obstacle> ();

		public override void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{
			base.Draw (tileDrawer, tilePool);
			
			foreach (var obstacle in obstacles) {
				tileDrawer.DrawTerrain (obstacle.tile, at: obstacle.position);
			}
		}
	}
}