using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class DelayedRevival : InstantRevival
	{
		public float delay = 2f;
		// Use this for initialization
		protected override void OnObjectDied (GameObject sender)
		{
			StartCoroutine (DelayedRevivalCoroutine (sender));
		}

		IEnumerator DelayedRevivalCoroutine (GameObject sender)
		{
			yield return new WaitForSeconds (delay);
			base.OnObjectDied (sender);
		}
	}
}