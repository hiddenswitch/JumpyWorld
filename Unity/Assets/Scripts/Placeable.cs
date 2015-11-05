using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class Placeable : GridLocateable, IBoundsGrid
	{
		public virtual Bounds BoundsGrid {
			get {
				return new Bounds (Tile.ToGrid (transform.position), Tile.ToGrid (transform.lossyScale));
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
	}
}