using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public enum HolePattern
	{
		None = 0,
		Random = 1,
		Checkers = 2,
		Center = 4,
	}

	public class HolesForRoom : ObstacleGenerator
	{
		[BitMaskAttribute(typeof(HolePattern))]
		public HolePattern holePattern = HolePattern.Random;

		public float holeProbability = 0.25f;
		public int checkerSpacing = 3;
		public int centerThresholdNorthSouth = 3;
		public int centerThresholdEastWest = 5;

		public override void Generate (int seed)
		{
			base.Generate (seed);

			foreach (var point in Floor.Rectangle(room.size,1f,height)) {
				var groundPosition = new Vector3(point.position.x, 0, point.position.z);

				// randomly adds holes with probability
				if ((holePattern & HolePattern.Random) == HolePattern.Random) {
					if (Random.Range(0.0f, 1.0f) < holeProbability) {
						var obstacle = new Obstacle() {
							position = groundPosition,
							tile = tilePool.defaultEmpty
						};
						obstacles.Add (obstacle);
					}
				}

				// adds holes on a grid with spacing defined by the user
				if ((holePattern & HolePattern.Checkers) == HolePattern.Checkers) {
					if (point.position.x % checkerSpacing == 0
					    && point.position.z % checkerSpacing == 0) {
						var obstacle = new Obstacle() {
							position = groundPosition,
							tile = tilePool.defaultEmpty
						};
						obstacles.Add (obstacle);
					}
				}

				// creates a rectangular hole in the center of the room
				if ((holePattern & HolePattern.Center) == HolePattern.Center) {
					if (point.distanceFromCenter[Vector4DirectionIndex.Vector4North] < centerThresholdNorthSouth
					    && point.distanceFromCenter[Vector4DirectionIndex.Vector4East] < centerThresholdEastWest) {
						var obstacle = new Obstacle() {
							position = groundPosition,
							tile = tilePool.defaultEmpty
						};
						obstacles.Add (obstacle);
					}
				}
			}
		}
	}
}