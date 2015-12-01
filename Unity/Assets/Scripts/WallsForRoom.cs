using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	/// <summary>
	/// Walls for rooms expects to be called after all rooms have actually been added, since its doors logic depends
	/// on knowing where all the possible floors are.
	/// </summary>
	public class WallsForRoom : ObstacleGenerator
	{
		[BitMaskAttribute(typeof(Directions))]
		public Directions
			wallsForSides = Directions.North | Directions.East | Directions.South | Directions.West;
		public bool openingsForDoors = true;

		public override void Generate (int seed)
		{
			base.Generate (seed);
			var floorPoints = new HashSet<Floor.RectanglePoint> ((int)(room.size.width * room.size.height));

			// Convert to hash set for easy lookups
			foreach (var point in floorPoints) {
				floorPoints.Add (Floor.Rectangle (room.size, 1f, height));
			}

			foreach (var point in floorPoints) {
				// In order to determine whether or not there should be a hole for a door, check for a T
				// pattern or a cross pattern of floors. Add on Vector3.down since we're checking below us
				// for floor
				var hasCrossPattern = tileDrawer.Contains (point.position + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.left + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.right + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.forward + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.back + Vector3.down);
				// Encode the four T patterns. All three T points must NOT belong on the floor
				var hasTeePattern = tileDrawer.Contains (point.position + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.left + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.right + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.forward + Vector3.down);
				hasTeePattern = hasTeePattern || tileDrawer.Contains (point.position + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.left + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.right + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.back + Vector3.down);
				hasTeePattern = hasTeePattern || tileDrawer.Contains (point.position + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.forward + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.right + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.back + Vector3.down);
				hasTeePattern = hasTeePattern || tileDrawer.Contains (point.position + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.forward + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.left + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.back + Vector3.down);

				// If we want to save openings for doors and we detected a floor pattern where a door should go (or
				// we're bordering another room's walls) don't build the opening.
				if (openingsForDoors
					&& (hasTeePattern || hasCrossPattern)) {
					continue;
				}

				if (point.isBorder
					&& (wallsForSides & point.side) > 0) {
					var obstacle = new Obstacle () {
						position = point.position,
						tile = tilePool.defaultGround
					};
					obstacles.Add (obstacle);

				}
			}
		}
	}
}