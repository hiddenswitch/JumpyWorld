using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[RequireComponent(typeof(CharacterController))]
	public class MovesAlongPath : MonoBehaviour
	{
		//<summary>
		/// Path in world space
		/// </summary>
		public Vector3[] path;
		CharacterController characterController;
		public bool shouldTeleportToStartOfPath;

		// speed is > 0
		public float speed;
		private int pathIndex = -1;
		private float arrivedThreshold;

		// Use this for initialization
		void Start ()
		{
			this.characterController = this.GetComponent<CharacterController> ();
			arrivedThreshold = speed / 2 * Time.fixedDeltaTime;
			if (shouldTeleportToStartOfPath) {
				pathIndex = 0;
				transform.position = new Vector3 (path [0].x, transform.position.y, path [0].z);
			} 
			transform.forward = calculateForward (transform.position, path [0]);
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		void FixedUpdate ()
		{
			var timeForFrame = Time.fixedDeltaTime;
			var currentPosition = transform.position;
			var nextPosition = path [(pathIndex + 1) % (path.Length - 1)];
			var hasArrived = xzDistance (currentPosition, nextPosition) < arrivedThreshold;
			if (hasArrived) {
				// move to the nextPosition, update forward direction
				transform.position = new Vector3 (nextPosition.x, transform.position.y, nextPosition.z);
				pathIndex = (pathIndex + 1) % (path.Length - 1);
				transform.forward = calculateForward (nextPosition, path [(pathIndex + 1) % (path.Length - 1)]);
			} 
			// move the character forward
			var velocity = transform.forward * speed;
			characterController.Move (velocity * timeForFrame);
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.blue;
			for (var i = 0; i < path.Length - 1; i++) {
				var p0 = path [i];
				p0.y += 0.5f;
				var p1 = path [i + 1];
				p1.y += 0.5f;
				Gizmos.DrawLine (p0, p1);
			}
		}

		// Returns the distance between p1 and p2 ignoring the y component
		float xzDistance (Vector3 p1, Vector3 p2)
		{
			var p1V2 = new Vector2 (p1.x, p1.z);
			var p2V2 = new Vector2 (p2.x, p2.z);
			return (p1V2 - p2V2).magnitude;
		}

		// Returns the forward vector corresponding to starting in p1 and ending at p2
		Vector3 calculateForward (Vector3 start, Vector3 end)
		{
			return (end - start).normalized;
		}
	}
}
