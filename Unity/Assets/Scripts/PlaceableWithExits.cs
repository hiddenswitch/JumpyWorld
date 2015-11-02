using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class PlaceableWithExits : Placeable
	{
		public Tile[] exits;

		public virtual Tile[] Exits {
			get {
				return exits;
			}
			set {
				exits = value;
			}
		}
	}
}
