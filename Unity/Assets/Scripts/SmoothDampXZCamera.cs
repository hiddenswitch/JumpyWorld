using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class SmoothDampXZCamera : MonoBehaviour
	{
		[Header ("Options")]
		public Transform
			target;
		public float smoothTime = 0.3f;
		public bool automaticallySetDifference = true;
		public Vector3 difference;
		public float yBottomClamp = 5f;
		[Header ("Runtime")]
		public Vector3
			velocity;

		// Use this for initialization
		public void Start ()
		{
			if (!automaticallySetDifference) {
				return;
			}
			
			if (target == null) {
				return;
			}
			
			difference = transform.position - target.position;
			automaticallySetDifference = false;
		}
		
		// Update is called once per frame
		void LateUpdate ()
		{
			if (target == null) {
				return;
			}
			if (automaticallySetDifference) {
				Start ();
			}
			var targetPosition = target.transform.position + difference;
			targetPosition.y = Mathf.Max (targetPosition.y, yBottomClamp);
			transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);
		}
	}
	
}