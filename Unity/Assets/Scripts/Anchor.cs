using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[System.Serializable]
	public struct Anchor : IPositionGrid, IDirections
	{
		public Vector3 position;
		public Directions directions;
        public Generator generator;


		public Vector3 PositionGrid {
			get {
				return position;
			}
			set {
				position = value;
			}
		}

		public Vector3 PositionGridLocal {
			get {
				return position;
			}
			set {
				position = value;
			}
		}

		public Directions Directions {
			get {
				return directions;
			}
			set {
				directions = value;
			}
		}

        public bool equals(Anchor other)
        {
            return position == other.position && directions == other.directions && generator == other.generator;
        }
	}
}