using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class DelayedRevival : MonoBehaviour
	{
		public float delay = 2f;
		public Killable killable;
		// Use this for initialization
		public void OnObjectDied (GameObject sender)
		{
			StartCoroutine (DelayedRevivalCoroutine (sender));
		}

		IEnumerator DelayedRevivalCoroutine (GameObject sender)
		{
			yield return new WaitForSeconds (delay);
			killable.Revive ();
		}
	}
}