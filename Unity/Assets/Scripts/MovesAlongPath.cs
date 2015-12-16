using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[RequireComponent(typeof(Rigidbody))]
	public class MovesAlongPath : Source<Vector3[]>
	{
		public Rigidbody rigidBody;

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
		private bool okayToRun = true;
	
		// Use this for initialization
		void Start ()
		{
			rigidBody = GetComponent<Rigidbody> ();
		}

		private void DelayedStart ()
		{
			// check that speed is positive
			if (speed <= 0) {
				okayToRun = false;
				Debug.LogError ("Speed needs to be positive");
			}
			
			// check if closed path
			if (path [0] != path [path.Length - 1]) {
				okayToRun = false;
				Debug.LogError ("Not a closed path.");
			}

			// check if path is rectilinear
			for (var i = 0; i < path.Length - 1; i++) {
				var p0 = path [i];
				var p1 = path [i + 1];
				if (!IsXZRectilinearLine (p0, p1)) {
					okayToRun = false;
					Debug.LogError ("Not rectilinear path.");
				}
			}
			
			if (okayToRun) {
				arrivedThreshold = speed / 2 * Time.fixedDeltaTime;
				height = transform.position.y + offset;
				if (shouldTeleportToStartOfPath) {
					pathIndex = 0;
					transform.position = new Vector3 (path [0].x, height, path [0].z);
					transform.forward = XZCalculateForward (path [0], path [1]);
				} else {
					// check that the starting position is rectilinear to the starting path position
					if (!IsXZRectilinearLine (transform.position, path [0])) {
						Debug.LogWarning ("Current position to start is not rectilinear.");
					}
					transform.forward = XZCalculateForward (transform.position, path [0]);
				}
			}
		}

		void FixedUpdate ()
		{
			if (okayToRun) {
				if (path.Length < 2) {
					return;
				}
				var currentPosition = transform.position;
				var nextPosition = path [(pathIndex + 1) % (path.Length - 1)];
				var hasArrived = XZDistance (currentPosition, nextPosition) < arrivedThreshold;
				if (hasArrived) {
					// move to the nextPosition, update forward direction
					transform.position = new Vector3 (nextPosition.x, height, nextPosition.z);
					pathIndex = (pathIndex + 1) % (path.Length - 1);
				} 
				transform.forward = XZCalculateForward (currentPosition, path [(pathIndex + 1) % (path.Length - 1)]);
				// move the character forward
				var velocity = transform.forward * speed;
				rigidBody.velocity = velocity;
			}
		}

		/// <summary>
		/// Calculates the XZ distance between p1 and p2
		/// </summary>
		/// <returns>The distance.</returns>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		float XZDistance (Vector3 p1, Vector3 p2)
		{
			var p1V2 = new Vector2 (p1.x, p1.z);
			var p2V2 = new Vector2 (p2.x, p2.z);
			return (p1V2 - p2V2).magnitude;
		}

		/// <summary>
		/// Calculates the forward vector corresponding to starting in p1 and 
		/// ending at p2 considering only the XZ plane
		/// </summary>
		/// <returns>The forward.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		Vector3 XZCalculateForward (Vector3 start, Vector3 end)
		{
			var newStart = new Vector3 (start.x, start.y, start.z);
			var newEnd = new Vector3 (end.x, height, end.z);
			return (newEnd - newStart).normalized;
		}

		/// <summary>
		/// Determines whether the line from start to end is rectilinear in the
		/// XZ planee
		/// </summary>
		/// <returns><c>true</c> if this instance is XZ rectilinear line at the specified start end; otherwise, <c>false</c>.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		bool IsXZRectilinearLine (Vector3 start, Vector3 end)
		{
			if (start.x != end.x) {
				return start.z == end.z;
			}
			return true;
		}
	}
}
