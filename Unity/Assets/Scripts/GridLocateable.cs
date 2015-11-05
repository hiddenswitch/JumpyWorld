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
				var forward = transform.forward.normalized;
				var northness = Vector3.Dot (forward, Vector3.forward);
				var eastness = Vector3.Dot (forward, Vector3.right);

				var direction = Directions.None;
				if (northness > 0.5f) {
					direction |= Directions.North;
				} else if (northness < -0.5f) {
					direction |= Directions.South;
				}

				if (eastness > 0.5f) {
					direction |= Directions.East;
				} else if (eastness < -0.5f) {
					direction |= Directions.West;
				}

				return direction;
			}
			set {
				var direction = Vector3.zero;

				if ((value & Directions.East) == Directions.East) {
					direction += Vector3.right;
				}
				if ((value & Directions.West) == Directions.West) {
					direction -= Vector3.right;
				}
				if ((value & Directions.North) == Directions.North) {
					direction += Vector3.forward;
				}
				if ((value & Directions.North) == Directions.North) {
					direction -= Vector3.forward;
				}

				transform.forward = direction;
			}
		}
	}
}
