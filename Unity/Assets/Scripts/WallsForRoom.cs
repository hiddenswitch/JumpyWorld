using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class WallsForRoom : ObstacleGenerator
	{
		[BitMaskAttribute(typeof(Directions))]
		public Directions
			wallsForSides = Directions.North | Directions.East | Directions.South | Directions.West;

		public override void Generate (int seed)
		{
			base.Generate (seed);
		
			foreach (var point in Floor.Rectangle(room.size,1f,height)) {
				if (point.isBorder
					&& (wallsForSides & point.side) > 0) {
					var obstacle = new Obstacle() {
						position = point.position,
						tile = tilePool.defaultGround
					};
					obstacles.Add (obstacle);

				}
			}
		}
	}
}