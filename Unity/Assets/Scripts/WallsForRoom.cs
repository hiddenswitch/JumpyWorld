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
			var floorPoints = new HashSet<Vector3> ();

			// Convert to hash set for easy lookups
			foreach (var point in Floor.Rectangle(room.size, step:1, y: height)) {
				floorPoints.Add (point.position);
			}

			foreach (var point in Floor.Rectangle(room.size, step:1, y: height)) {
				// In order to determine whether or not there should be a hole for a door, check for a T
				// pattern or a cross pattern of floors. Add on Vector3.down since we're checking below us
				// for floor
				var hasCrossPattern = tileDrawer.Contains (point.position + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.left + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.right + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.forward + Vector3.down)
					&& tileDrawer.Contains (point.position + Vector3.back + Vector3.down);

//				// Encode the four T patterns.
//				var teePoints = new Vector3[][] {
//					new Vector3[] {
//						Vector3.zero,
//						Vector3.left,
//						Vector3.right,
//						Vector3.forward
//					},
//					new Vector3[] {
//						Vector3.zero,
//						Vector3.left,
//						Vector3.right,
//						Vector3.back
//					},
//					new Vector3[] {
//						Vector3.zero,
//						Vector3.forward,
//						Vector3.right,
//						Vector3.back
//					},
//					new Vector3[] {
//						Vector3.zero,
//						Vector3.forward,
//						Vector3.left,
//						Vector3.back
//					},
//				};
//
//				// In order for there to be a T pattern, all four points (the three "tee" points plus the current point
//				// must exist and at least one must not belong to this floor.
//				var hasAnyTeePattern = false;
//				foreach (var tee in teePoints) {
//					var isTeePattern = true;
//					var allBelongToFloor = true;
//
//					foreach (var teePoint in tee) {
//						var testPosition = point.position + teePoint + Vector3.down;
//						// All four points must exist
//						if (!tileDrawer.Contains (testPosition)) {
//							isTeePattern = false;
//							break;
//						}
//
//						if (!floorPoints.Contains(testPosition)) {
//							allBelongToFloor = false;
//						}
//					}
//
//					if (allBelongToFloor) {
//						isTeePattern = false;
//					}
//
//					if (isTeePattern) {
//						hasAnyTeePattern = true;
//						break;
//					}
//				}

				// If we want to save openings for doors and we detected a floor pattern where a door should go (or
				// we're bordering another room's walls) don't build the opening.
				if (openingsForDoors
				    && ( /*hasAnyTeePattern || */ hasCrossPattern)) {
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