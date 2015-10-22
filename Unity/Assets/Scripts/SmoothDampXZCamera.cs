using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class SmoothDampXZCamera : MonoBehaviour
	{
		[Header("Options")]
		public Transform
			target;
		public float smoothTime = 0.3f;
		public Vector3 difference;
		[Header("Runtime")]
		public Vector3
			velocity;
		public bool needsStart = true;
		
		// Use this for initialization
		public void Start ()
		{
			if (!needsStart) {
				return;
			}
			
			if (target == null) {
				return;
			}
			
			difference = transform.position - target.position;
			needsStart = false;
		}
		
		// Update is called once per frame
		void LateUpdate ()
		{
			if (target == null) {
				return;
			}
			if (needsStart) {
				Start ();
			}
			transform.position = Vector3.SmoothDamp (transform.position, target.transform.position + difference, ref velocity, smoothTime);
		}
	}
	
}