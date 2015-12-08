using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class FXOnTouch : MonoBehaviour
	{
		public LayerMask triggersWith;
		public GameObject effect;
		public Vector3 offset;
		public Vector3 scale;
		public Quaternion rotation;

		public virtual void OnTriggerEnter (Collider other)
		{
			if (((1 << other.gameObject.layer) & triggersWith.value) > 0) {
				var instance = CFX_SpawnSystem.GetNextObject (effect);
				instance.transform.position = transform.position + offset;
				instance.transform.localScale = scale;
				instance.transform.rotation = rotation;
			}
		}
	}
}