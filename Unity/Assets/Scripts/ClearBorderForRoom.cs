using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{	
	public class ClearBorderForRoom : ObstacleGenerator
	{	
		public override void Generate (int seed)
		{
			base.Generate (seed);
			
			foreach (var point in Floor.Rectangle(room.size,1f,height)) {
				
				if (point.isBorder) {
					var openObstacle = new Obstacle() {
						position = point.position,
						tile = tilePool.defaultEmpty
					};
					obstacles.Add (openObstacle);

					var fillGround = new Obstacle() {
						position = new Vector3(point.position.x, 0f, point.position.z),
						tile = tilePool.defaultGround
					};
					obstacles.Add (fillGround);
				}
			}
		}
	}
}