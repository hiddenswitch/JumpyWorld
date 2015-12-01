using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ResetPositionOnDeath : MonoBehaviour
	{
		public HasStartPosition objectWithPosition;
		public float delay = 2;

		void Update ()
		{
		}

		void OnObjectDied (GameObject sender)
		{
			if (enabled
				&& sender == objectWithPosition.gameObject) {
				StartCoroutine (DelayedReset (delay));
			}
		}

		IEnumerator DelayedReset (float delay)
		{
			yield return new WaitForSeconds(delay);
			objectWithPosition.ResetPosition ();
		}
	}

}