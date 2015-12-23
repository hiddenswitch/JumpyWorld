using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RandomPlacerForRoom : Generator, IForFloor
	{
		public Floor room;
		public AnimationCurve xDensity;
		public AnimationCurve zDensity;
		public float height = 1f;
		public float step = 1f;
        public bool overrideCurrent = false;

		public override void Draw (TileDrawer tileDrawer=null, TilePool tilePool=null)
		{
			tilePool = tilePool ?? this.tilePool;
			tileDrawer = tileDrawer ?? this.tileDrawer;
			foreach (var point in Floor.Rectangle(room.size, step, height)) {
				var random = Random.value;
				var xz = Mathf.Sqrt (xDensity.Evaluate (Mathf.InverseLerp (room.size.xMin, room.size.xMax, point.position.x))
					* zDensity.Evaluate (Mathf.InverseLerp (room.size.yMin, room.size.yMax, point.position.z)));
				if (random < xz) {
					tileDrawer.DrawTerrain (prefab: tilePool.decorative [Random.Range (0, tilePool.decorative.Length - 1)], at: point.position, overwriteIfExists: overrideCurrent);
				}
			}
		}

		Floor IForFloor.floor {
			get {
				return this.room;
			}
			set {
				this.room = value;
			}
		}
	}
}