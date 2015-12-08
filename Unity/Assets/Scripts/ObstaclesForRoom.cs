using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{


	public class ObstaclesForRoom : ObstacleGenerator
	{


		public Directions side = Directions.North;

		public override void Generate (int seed)
		{
			base.Generate (seed);

			foreach (var point in Floor.Rectangle(room.size,1f,height)) {

				// XXXXXXXXXXXXXXX
				// X             X
				// XOXXXXXXXXXXXOX
				// X             X
				// XXXXXXXXXXXXXXX
				// Remove positions that are bordering the border so that the stripes
				// are passable

				var border1 = Vector4DirectionIndex.Vector4East;
				var border2 = Vector4DirectionIndex.Vector4West;
				var stripeAxis = 2;
				if (side == Directions.East
					|| side == Directions.West) {
					border1 = Vector4DirectionIndex.Vector4North;
					border2 = Vector4DirectionIndex.Vector4South;
					stripeAxis = 0;
				}

				// Is the current point that we're about to add on a stripe?
				var isOnStripe = point.position[stripeAxis] % 2 == 0;
				var isOnSide = (point.side & side) == side;
				if (isOnSide
					&& isOnStripe
					&& point.distanceFromBorders [border1] > 1
					&& point.distanceFromBorders [border2] > 1) {
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