using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class HasStartPosition : MonoBehaviour
	{
		public Vector3 startPosition = Vector3.zero;

		public void ResetPosition ()
		{
			transform.position = startPosition;
			var rb = GetComponent<Rigidbody> ();
			if (rb == null) {
				return;
			}
			rb.velocity = Vector3.zero;
		}
	}
}