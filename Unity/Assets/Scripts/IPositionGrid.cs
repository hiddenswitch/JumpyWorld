using UnityEngine;

namespace JumpyWorld
{
	public interface IPositionGrid
	{
		Vector3 PositionGrid {
			get;
			set;
		}
		
		Vector3 PositionGridLocal {
			get;
			set;
		}
	}
}