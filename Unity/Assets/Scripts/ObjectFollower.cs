using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[RequireComponent(typeof(DelayedTurnController))]
	[RequireComponent(typeof(SnapToCell))]
	public class ObjectFollower : MonoBehaviour
	{

		public float moveSpeed = 1f;
		public Transform target;
		public int rotationSpeed = 1;
		public Floor floor;
		public float stop = 0f;
		private Vector3 currentPt;

		// threshold for what is close enough in terms of when to enter and exit cells
		private float thresh1;

		// Use this for initialization
		void Start ()
		{
			currentPt = this.gameObject.transform.position;

			var delayedController = this.gameObject.GetComponent<DelayedTurnController> ();
			thresh1 = Mathf.Min (Mathf.Abs (delayedController.enterCellThreshold), Mathf.Abs (delayedController.exitCellThreshold));

			if (currentPt != RoundVector3XZ (currentPt)) {
				enabled = false;
				Debug.LogError ("The current game object's position must be integers");
			}
		}
		
		void FixedUpdate ()
		{
			var thresh2 = moveSpeed * Time.fixedDeltaTime;
			var threshold = Mathf.Min (thresh1, thresh2);
			var myTransform = this.gameObject.transform;

			var distanceFromTarget = 0f;
			var targetInBounds = false;

			// checking if the target is in the bounds of the floor
			if (target != null) {
				var bounds = floor.size;
				distanceFromTarget = Vector3.Distance (myTransform.position, target.position);
				targetInBounds = (bounds.xMin <= target.position.x) && 
					(target.position.x <= bounds.xMax) && 
					(bounds.yMin <= target.position.z) && 
					(target.position.z <= bounds.yMax);
			}
			
			// only update if the target is in bounds and not too close
			if (targetInBounds && distanceFromTarget >= stop) {
				var path = Hallway.BresenhamFilled (
					from: RoundVector3XZ (myTransform.position),
					to: RoundVector3XZ (target.position),
					shouldBalanceCorners: true);
			
				foreach (var point in path) {
					if (point != RoundVector3XZ (myTransform.position)) {
						
						Vector3 currentDirection = myTransform.forward.normalized;
						Vector3 direction = currentDirection;
						//Debug.Log (myTransform.position - RoundVector3XZ(myTransform.position));
						if (CloseEnough (myTransform.position, RoundVector3XZ (myTransform.position), threshold)) {
							direction = (point - RoundVector3XZ (myTransform.position)).normalized;
							direction = new Vector3 (direction.x, 0, direction.z);
							currentPt = point;

						}

						// transform the direction game object travels into a swipe
						if (currentDirection != direction) {
							if (direction == Vector3.left) {
								myTransform.gameObject.BroadcastMessage ("SwipeWest");
							}
							if (direction == Vector3.right) {
								myTransform.gameObject.BroadcastMessage ("SwipeEast");
							}
							if (direction == Vector3.forward) {
								myTransform.gameObject.BroadcastMessage ("SwipeNorth");
							}
							if (direction == Vector3.back) {
								myTransform.gameObject.BroadcastMessage ("SwipeSouth");
							}
						}

						// move the character forward
						updateObjectPosition (myTransform, direction);
						break;
					}			
				}
			}

			// used for moving the character to an integer point after target leaves floor
			else if ((!CloseEnough (currentPt, myTransform.position, threshold)) && distanceFromTarget >= stop) {
				Vector3 newDirection = RoundVector3XZ ((currentPt - myTransform.position).normalized);
				if (newDirection == Vector3.left || 
					newDirection == Vector3.right || 
					newDirection == Vector3.forward || 
					newDirection == Vector3.back) {
					updateObjectPosition (myTransform, newDirection);
				}
			}
		}

		void updateObjectPosition (Transform transform, Vector3 direction)
		{
			transform.forward = direction;
			var velocity = transform.forward * moveSpeed;
			transform.position = transform.position + velocity * Time.fixedDeltaTime;
		}

		/// <summary>
		/// Rounds the x and z coordinates of the vector3 to integers.
		/// </summary>
		/// <returns>The new xz rounded vector3.</returns>
		/// <param name="v">V.</param>
		Vector3 RoundVector3XZ (Vector3 v)
		{
			return new Vector3 (Mathf.Round (v.x), v.y, Mathf.Round (v.z));
		}

		/// <summary>
		/// Checks if v1 and v2 are less than threshold apart
		/// </summary>
		/// <returns><c>true</c>, if v1 and v2 are less than threshold apart, <c>false</c> otherwise.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		/// <param name="threshold">Threshold.</param>
		bool CloseEnough (Vector3 v1, Vector3 v2, float threshold)
		{
			return (v1 - v2).magnitude < threshold;
		}

	}
}


