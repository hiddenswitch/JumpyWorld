using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class HolesForRoom : Generator
	{
		public Floor room;
		public float height = 1f;
		[Header("Runtime")]
		public HashSet<Vector3>
			holePositions = new HashSet<Vector3> ();

		public override void Generate (int seed)
		{
			base.Generate (seed);

			var groundOffest = new Vector3(0, 1, 0);
			
			foreach (var point in Floor.Rectangle(room.size,1f,height)) {
				
				if (Random.Range(0.0f, 1.0f) > 0.49f) {
					var newPos = new Vector3(point.position.x, 0, point.position.z);
					holePositions.Add(newPos);
				}
			}
		}
		
		public override void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{
			base.Draw (tileDrawer, tilePool);



			foreach (var point in holePositions) {
				tileDrawer.DrawTerrain (tilePool.defaultEmpty, at: point);
			}
		}
		
		public override void OnDrawGizmos ()
		{
			base.OnDrawGizmos ();
			
			
		}
	}
}