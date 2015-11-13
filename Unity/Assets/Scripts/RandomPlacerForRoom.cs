using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RandomPlacerForRoom : Generator
	{
		public Room room;
		public AnimationCurve xDensity;
		public AnimationCurve zDensity;
		public float height = 1f;
		public float step = 1f;

		public override void Draw (TileDrawer tileDrawer=null, TilePool tilePool=null)
		{
			tilePool = tilePool ?? this.tilePool;
			tileDrawer = tileDrawer ?? this.tileDrawer;
			foreach (var point in Room.Rectangle(room.size, step, height)) {
				var random = Random.value;
				var xz = Mathf.Sqrt (xDensity.Evaluate (Mathf.InverseLerp (room.size.xMin, room.size.xMax, point.position.x))
					* zDensity.Evaluate (Mathf.InverseLerp (room.size.yMin, room.size.yMax, point.position.z)));
				if (random < xz) {
					tileDrawer.DrawTerrain (tile: tilePool.decorative [Random.Range (0, tilePool.decorative.Length - 1)], at: point.position);
				}
			}
		}
	}
}