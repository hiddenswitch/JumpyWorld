using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class MicroMonstersAnimationController : MonoBehaviour
	{
		public Animator animator;
		public string speedParameter = "speed_f";
		private Vector3 previous;
		private float speed = 0;

		// Use this for initialization
		void Start ()
		{
			previous = transform.position;
		}
	
		void FixedUpdate ()
		{
			// updates the speed of the ghost
			speed = ((transform.position - previous).magnitude) / Time.fixedDeltaTime;
			previous = transform.position;
			animator.SetFloat (speedParameter, speed);
		}
	}
}
