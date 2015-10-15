using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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
		
		void SwipeNorth ()
		{
			if (!isAlive) {
				return;
			}
			moving = true;
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		}
		
		void SwipeWest ()
		{
			if (!isAlive) {
				return;
			}
			moving = true;
			transform.rotation = Quaternion.Euler (0f, -90f, 0f);
		}
		
		void SwipeEast ()
		{
			if (!isAlive) {
				return;
			}
			moving = true;
			transform.rotation = Quaternion.Euler (0f, 90f, 0f);
		}
		
		void SwipeSouth ()
		{
			if (!isAlive) {
				return;
			}
			moving = true;
			transform.rotation = Quaternion.Euler (0f, 180f, 0f);
		}
	}
}