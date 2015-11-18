using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ResetPositionOnDeath : MonoBehaviour
	{
		public HasStartPosition objectWithPosition;

		void Update ()
		{
		}

		void OnObjectDied (GameObject sender)
		{
			if (enabled
				&& sender == objectWithPosition.gameObject) {
				objectWithPosition.ResetPosition ();
			}
		}
	}

}