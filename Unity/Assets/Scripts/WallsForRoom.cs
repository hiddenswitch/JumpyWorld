using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class WallsForRoom : Generator
	{
		public Room room;
		public float height = 1f;
		[BitMaskAttribute(typeof(Directions))]
		public Directions
			wallsForSides = Directions.North | Directions.East | Directions.South | Directions.West;
		[Header("Runtime")]
		public HashSet<Vector3>
			obstaclePositions = new HashSet<Vector3> ();

		public override void Generate (int seed)
		{
			base.Generate (seed);
		
			foreach (var point in Room.Rectangle(room.size,1f,height)) {
				if (point.isBorder
					&& (wallsForSides & point.side) > 0) {
					obstaclePositions.Add (point.position);
				}
			}
		}

		public override void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{
			base.Draw (tileDrawer, tilePool);
		
			foreach (var point in obstaclePositions) {
				tileDrawer.DrawTerrain (tilePool.defaultGround, at: point);
			}
		}
	}
}