using UnityEngine;

namespace JumpyWorld
{
	public class Tile : MonoBehaviour, IDirections, IPositionGrid
	{
		public static float gridSize = 1;
		[BitMaskAttribute (typeof(Directions))]
		public Directions
			directions;

		public Vector3 pivot = new Vector3 (0f, 0f, 0f);
		public Vector3 size = new Vector3 (1f, 1f, 1f);

		public static Vector3 ToGrid (Vector3 position)
		{
			return new Vector3 (Mathf.Round (position.x / gridSize), 
				Mathf.Floor (position.y / gridSize), 
				Mathf.Round (position.z / gridSize));
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