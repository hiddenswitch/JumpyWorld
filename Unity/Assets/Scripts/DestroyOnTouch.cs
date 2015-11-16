using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class DestroyOnTouch : MonoBehaviour
	{
		public LayerMask triggersWith;

		void OnTriggerEnter (Collider other)
		{
			if (((1 << other.gameObject.layer) & triggersWith.value) > 0) {
				Destroy (this.gameObject);
			}
		}
	}
}
