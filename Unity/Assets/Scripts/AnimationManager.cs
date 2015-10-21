﻿using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class AnimationManager : MonoBehaviour
	{
		public string speedParameter;
		public string deathParameter = "Death_b";
		public Animator animator;
		public CharacterController controller;
        public Rigidbody rigidBody;
		public float speedScale = 1.0f;
		[Header("Runtime")]
		public bool
			isAlive = true;
		

		void OnKill ()
		{
			isAlive = false;
		}
		
		// Update is called once per frame
		void LateUpdate ()
		{
			if (animator == null
			    || controller == null) {
				return;
			}
			
			if (isAlive) {
                if (rigidBody)
                {
                    animator.SetFloat(speedParameter, rigidBody.velocity.magnitude * speedScale);
                } else
                {
                    animator.SetFloat (speedParameter, controller.velocity.magnitude * speedScale);
                }
            } else {
				animator.SetFloat (speedParameter, 0f);
				animator.SetBool (deathParameter, true);
			}
		}
	}
}
