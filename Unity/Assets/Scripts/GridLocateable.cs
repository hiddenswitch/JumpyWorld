using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class GridLocateable : MonoBehaviour, IPositionGrid, IDirections
	{
		public virtual Vector3 PositionGrid {
			get {
				return Tile.ToGrid (transform.position);
			}
			set {
				transform.position = Tile.gridSize * value;
			}
		}

		public virtual Vector3 PositionGridLocal {
			get {
				return Tile.ToGrid (transform.localPosition);
			}
			set {
				transform.localPosition = Tile.gridSize * value;
			}
		}

		public virtual Directions Directions {
			get {
				return transform.forward.ToDirection ();
			}
			set {
				transform.forward = value.ToVector ();
			}
		}
	}
}
