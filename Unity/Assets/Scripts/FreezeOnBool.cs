using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[RequireComponent(typeof(Rigidbody))]
	public class FreezeOnBool : MonoBehaviour
	{
		public PinBool pinBool;
		Rigidbody rb;
		bool previousIsLoading = true;

		// Use this for initialization
		void Start ()
		{
			if (pinBool == null) {
				this.enabled = false;
			}
			rb = gameObject.GetComponent<Rigidbody> ();
			previousIsLoading = pinBool.value;
			rb.isKinematic = pinBool.value;
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
			if (pinBool.value != previousIsLoading) {
				rb.isKinematic = pinBool.value;
			}

			previousIsLoading = pinBool.value;
		}
	}
}
