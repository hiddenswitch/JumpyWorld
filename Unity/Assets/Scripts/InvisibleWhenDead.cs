using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class InvisibleWhenDead : MonoBehaviour
	{
		public GameObject art;
		public float delay = 0.5f;

		void OnObjectDied ()
		{
			art.SetActive (false);
		}

		void OnObjectRevived ()
		{
			StartCoroutine (SetActiveTrue ());
		}

		IEnumerator SetActiveTrue ()
		{
			yield return new WaitForSeconds (delay);
			art.SetActive (true);
		}
	}
}