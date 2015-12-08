using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public enum ObstaclePattern
	{
		None = 0,
		CornerEdges = 1,
		CenterCross = 2,
		Diagonals = 4,
		Stripes = 8
	}
	
	public class ObstaclePlacerForRoom : ObstacleGenerator
	{
		[BitMaskAttribute(typeof(ObstaclePattern))]
		public ObstaclePattern obstaclePattern = ObstaclePattern.CenterCross;

		public float step = 1f;

		public int crossRadius = 5;
		public int crossCenterClearingRadius = 2;
		public int diagonalsRadius = 6;
		public int diagonalCenterClearingRadius = 2;
		public int cornerExtent = 4;

		public override void Generate (int seed)
		{
			base.Generate (seed);
			tilePool = tilePool ?? this.tilePool;
			tileDrawer = tileDrawer ?? this.tileDrawer;

			foreach (var point in Floor.Rectangle(room.size, step, height)) {

				float vertCenterOffset = point.distanceFromCenter[Vector4DirectionIndex.Vector4North];
				float horzCenterOffset = point.distanceFromCenter[Vector4DirectionIndex.Vector4East];

				bool isOnCenterVert = horzCenterOffset < 1;
				bool isOnCenterHorz = vertCenterOffset < 1;

				if ((obstaclePattern & ObstaclePattern.CenterCross) == ObstaclePattern.CenterCross) {

					// selects points within the north-south centered part of the cross
					bool isWithinCrossRadiusVert = point.distanceFromCenter[Vector4DirectionIndex.Vector4North] < crossRadius
						&& point.distanceFromCenter[Vector4DirectionIndex.Vector4North] >= crossCenterClearingRadius;
					// selects points within the east-west centered part of the cross
					bool isWithinCrossRadiusHorz = point.distanceFromCenter[Vector4DirectionIndex.Vector4East] < crossRadius
						&& point.distanceFromCenter[Vector4DirectionIndex.Vector4East] >= crossCenterClearingRadius;

					// add obstacles for any selected points
					if ((isOnCenterVert && isWithinCrossRadiusVert)
					    || (isOnCenterHorz && isWithinCrossRadiusHorz)) {
						obstacles.Add(new Obstacle() {
							position = point.position,
							tile = tilePool.defaultGround
						});
					}
				}

				if ((obstaclePattern & ObstaclePattern.Diagonals) == ObstaclePattern.Diagonals) {

					bool isOnDiagonal = vertCenterOffset == horzCenterOffset;
					bool isWithinRadius = vertCenterOffset < diagonalsRadius && vertCenterOffset >= diagonalCenterClearingRadius 
						&& horzCenterOffset < diagonalsRadius && diagonalsRadius >= diagonalCenterClearingRadius;

					if (isOnDiagonal && isWithinRadius) {
						obstacles.Add (new Obstacle() {
							position = point.position,
							tile = tilePool.defaultGround
						});
					}
				}

				if ((obstaclePattern & ObstaclePattern.CornerEdges) == ObstaclePattern.CornerEdges) {

					float spaceBetweenCorners = (room.size.width - 2 * cornerExtent);

					if (point.isBorder
					    && vertCenterOffset >= spaceBetweenCorners/2
					    && horzCenterOffset >= spaceBetweenCorners/2) {

						obstacles.Add (new Obstacle() {
							position = point.position,
							tile = tilePool.defaultGround
						});

					}

				}

			}
		}
	}
}