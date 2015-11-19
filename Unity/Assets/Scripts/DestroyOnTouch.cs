using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class DestroyOnTouch : MonoBehaviour
	{
		public LayerMask triggersWith;

		public virtual void OnTriggerEnter (Collider other)
		{
			if (((1 << other.gameObject.layer) & triggersWith.value) > 0) {
				Destroy (this.gameObject);
			}
		}
	}
}
