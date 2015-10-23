using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class MoveController : MonoBehaviour
	{
		CharacterController controller;
		public float speed = 0.85f;
		[Header("Runtime")]
		public bool
			moving = true;
		public bool isAlive = true;

		// Use this for initialization
		void Start ()
		{
			// Move controller is disabled for non-local players.
			/*
			if (!isLocalPlayer) {
				enabled = false;
				return;
			}*/
			controller = GetComponent<CharacterController> ();

		}

		void Update ()
		{

		}
		
		void HandleOnGameOver ()
		{
			enabled = false;
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
			if (moving && isAlive) {
				controller.Move (transform.forward * Time.fixedDeltaTime * speed);
			} else {
				controller.Move (Vector3.zero);
			}
		}

		void TurnDirection (Vector3 targetRotation)
		{
			//transform.rotation = Quaternion.Euler(targetRotation);
			StartCoroutine (SmoothTurn (targetRotation));
		}

		IEnumerator SmoothTurn (Vector3 targetRotationEuler)
		{
			Quaternion startRotation = transform.rotation;
			for (int i = 0; i < 5; i++) {
				transform.rotation = Quaternion.Lerp (startRotation, Quaternion.Euler (targetRotationEuler), i / (float)4);
				yield return new WaitForEndOfFrame ();
			}
		}
	}
}