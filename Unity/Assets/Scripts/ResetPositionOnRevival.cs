using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ResetPositionOnRevival : MonoBehaviour
	{
		public HasStartPosition objectWithPosition;
		public float delay = 2;

		void Update ()
		{
		}

		void OnObjectRevived (GameObject sender)
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