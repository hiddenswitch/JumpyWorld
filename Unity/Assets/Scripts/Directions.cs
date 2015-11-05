using UnityEngine;

namespace JumpyWorld
{
	[System.Flags]
	public enum Directions
	{
		None = 0,
		North = 1,
		East = 2,
		South = 4,
		West = 8
	}

	public static class DirectionsExtensions
	{
		public static Vector3 ToVector (this Directions value)
		{
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
			if ((value & Directions.South) == Directions.South) {
				direction -= Vector3.forward;
			}
			
			return direction.normalized;
		}

		public static Directions ToDirection (this Vector3 forward)
		{
			var northness = Vector3.Dot (forward, Vector3.forward);
			var eastness = Vector3.Dot (forward, Vector3.right);
			var sqrt2 = Mathf.Sqrt (2);
			var direction = Directions.None;
			if (northness > sqrt2) {
				direction |= Directions.North;
			} else if (northness < -sqrt2) {
				direction |= Directions.South;
			}
			
			if (eastness > sqrt2) {
				direction |= Directions.East;
			} else if (eastness < -sqrt2) {
				direction |= Directions.West;
			}
			
			return direction;
		}
	}
}