using UnityEngine;

namespace JumpyWorld
{
	public class Tile : MonoBehaviour, IDirections, IPositionGrid
	{
		public static float gridSize = 1;
		[BitMaskAttribute(typeof(Directions))]
		public Directions
			directions;

		public static Vector3 ToGrid (Vector3 position)
		{
			return new Vector3 (Mathf.Floor (position.x / gridSize), Mathf.Floor (position.y / gridSize), Mathf.Floor (position.z / gridSize));
		}

		public virtual Vector3 PositionGrid {
			get {
				return ToGrid (transform.position);
			}
			set {
				transform.position = gridSize * value;
			}
		}

		public virtual Vector3 PositionGridLocal {
			get {
				return ToGrid (transform.localPosition);
			}
			set {
				transform.localPosition = gridSize * value;
			}
		}

		public virtual Directions Directions {
			get {
				return directions;
			}
			set {
				directions = value;
			}
		}
	}
}