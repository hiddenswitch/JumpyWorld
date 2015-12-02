using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[RequireComponent(typeof(Rigidbody))]
	public class MovesAlongPath : Source<Vector3[]>
	{
		Rigidbody rigidbody;

		public Vector3[] path {
			get {
				return this.value;
			}
			set {
				this.value = value;
			}
		}

		public bool shouldTeleportToStartOfPath;

		// speed is > 0
		public float speed;
		private int pathIndex = -1;
		private float arrivedThreshold;
		private float height;
		public float offset = 0f;

		// Use this for initialization
		void Start ()
		{
		}

		private void DelayedStart ()
		{
			// check that speed is positive
			if (speed <= 0) {
				enabled = false;
				Debug.LogError ("Speed needs to be positive");
			}
			
			// check if closed path
			if (path [0] != path [path.Length - 1]) {
				enabled = false;
				Debug.LogError ("Not a closed path.");
			}

			// check if path is rectilinear
			for (var i = 0; i < path.Length - 1; i++) {
				//Debug.Log (path[i]);
				var p0 = path [i];
				var p1 = path [i + 1];
				if (!isXZRectilinearLine (p0, p1)) {
					enabled = false;
					Debug.LogError ("Not rectilinear path.");
				}
			}
			
			this.rigidbody = this.GetComponent<Rigidbody> ();
			arrivedThreshold = speed / 2 * Time.fixedDeltaTime;
			height = transform.position.y + offset;
			if (shouldTeleportToStartOfPath) {
				pathIndex = 0;
				transform.position = new Vector3 (path [0].x, height, path [0].z);
			} else {
				// check that the starting position is rectilinear to the starting path position
				if (!isXZRectilinearLine (transform.position, path [0])) {
					Debug.LogWarning ("Current position to start is not rectilinear.");
				}
			}
			transform.forward = xzCalculateForward (transform.position, path [0]);

		}

		void FixedUpdate ()
		{
			if (path.Length < 2) {
				return;
			}
			var currentPosition = transform.position;
			var nextPosition = path [(pathIndex + 1) % (path.Length - 1)];
			var hasArrived = xzDistance (currentPosition, nextPosition) < arrivedThreshold;
			if (hasArrived) {
				// move to the nextPosition, update forward direction
				transform.position = new Vector3 (nextPosition.x, transform.position.y, nextPosition.z);
				pathIndex = (pathIndex + 1) % (path.Length - 1);
			} 
			transform.forward = xzCalculateForward (currentPosition, path [(pathIndex + 1) % (path.Length - 1)]);
			// move the character forward
			var velocity = transform.forward * speed;
			rigidbody.velocity = velocity;
		}

		// Returns the distance between p1 and p2 ignoring the y component
		float xzDistance (Vector3 p1, Vector3 p2)
		{
			var p1V2 = new Vector2 (p1.x, p1.z);
			var p2V2 = new Vector2 (p2.x, p2.z);
			return (p1V2 - p2V2).magnitude;
		}

		// Returns the forward vector corresponding to starting in p1 and ending at p2
		// ignoring the y component
		Vector3 xzCalculateForward (Vector3 start, Vector3 end)
		{
			var newStart = new Vector3 (start.x, start.y, start.z);
			var newEnd = new Vector3 (end.x, height, end.z);
			return (newEnd - newStart).normalized;
		}

		// Returns true if start and end make a rectilinear line on the xz plane
		bool isXZRectilinearLine (Vector3 start, Vector3 end)
		{
			if (start.x != end.x) {
				return start.z == end.z;
			}
			return true;
		}
	}
}
