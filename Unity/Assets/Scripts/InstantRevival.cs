using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class InstantRevival : MonoBehaviour
	{

		// Use this for initialization
		void OnObjectDied (GameObject sender)
		{
			var death = sender.GetComponent<DiesBelowDepth> ();
			if (death == null) {
				return;
			}
			death.ResetDies ();
		}
	}
}