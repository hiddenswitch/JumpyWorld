using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class FreezeOnDeath : MonoBehaviour
	{
		public bool frozen;
		public RigidBodyMoveController moveController;
		public bool enableGravityOnRevival = true;
		public float delay = 0.2f;

		void OnObjectDied (GameObject sender)
		{
			moveController.moving = false;
			moveController.rigidBody.velocity = Vector3.zero;
			moveController.rigidBody.useGravity = !enableGravityOnRevival;
		}

		void OnObjectRevived (GameObject sender)
		{
			StartCoroutine (DelayedUnfreeze ());
		}

		IEnumerator DelayedUnfreeze ()
		{
			yield return new WaitForSeconds (delay);
			moveController.moving = true;
			moveController.rigidBody.useGravity = enableGravityOnRevival;
		}
	}

}