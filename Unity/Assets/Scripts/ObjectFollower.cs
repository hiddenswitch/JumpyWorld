using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		private List<Vector3> pathP = new List<Vector3> ();
		private Vector3 currentPt;
		private float thresh1;

		// Use this for initialization
		void Start ()
		{
			// for testing purposes only!
			target.gameObject.BroadcastMessage ("DelayedStart");
			currentPt = this.gameObject.transform.position;

			var delayedController = this.gameObject.GetComponent<DelayedTurnController> ();
			thresh1 = Mathf.Min (Mathf.Abs (delayedController.enterCellThreshold), Mathf.Abs (delayedController.exitCellThreshold));

			if (currentPt != RoundVector3 (currentPt)) {
				enabled = false;
				Debug.LogError ("The current game object's position must be integers");
			}
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
			pathP.Clear ();
			var thresh2 = moveSpeed / 2 * Time.fixedDeltaTime;
			var threshold = Mathf.Min (thresh1, thresh2);
			var myTransform = this.gameObject.transform;

			var distance = 0f;
			var targetInBounds = false;

			if (target != null) {
				var bounds = floor.size;
				distance = Vector3.Distance (myTransform.position, target.position);
				targetInBounds = (bounds.xMin <= target.position.x) && 
					(target.position.x <= bounds.xMax) && 
					(bounds.yMin <= target.position.z) && 
					(target.position.z <= bounds.yMax);
			}
			
			// only update if the target is in bounds and not too close
			if (targetInBounds && distance >= stop) {
				var path = Hallway.BresenhamFilled (
					from: RoundVector3 (myTransform.position),
					to: RoundVector3 (target.position),
					shouldBalanceCorners: true);
				
				pathP = new List<Vector3> (path);

				foreach (var point in path) {
					if (point != RoundVector3 (myTransform.position)) {
						
						Vector3 currentDirection = myTransform.forward.normalized;
						Vector3 direction = currentDirection;
						if (CloseEnough (myTransform.position, RoundVector3 (myTransform.position), threshold)) {
							direction = (point - RoundVector3 (myTransform.position)).normalized;
							direction = new Vector3 (direction.x, 0, direction.z);
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
						myTransform.forward = direction;
						var velocity = myTransform.forward * moveSpeed;
						myTransform.position = myTransform.position + velocity * Time.deltaTime;
						currentPt = point;
						break;
					}			
				}
			}

			// used for moving the character to an integer point
			else if ((!CloseEnough (currentPt, myTransform.position, threshold)) && distance >= stop) {
				Vector3 newDirection = RoundVector3 ((currentPt - myTransform.position).normalized);
				if (newDirection == Vector3.left || newDirection == Vector3.right || newDirection == Vector3.forward || newDirection == Vector3.back) {
					myTransform.forward = newDirection;
					var velocity = myTransform.forward * moveSpeed;
					myTransform.position = myTransform.position + velocity * Time.deltaTime;
				}
			}
		}

		/// <summary>
		/// Rounds the vector3 to integers.
		/// </summary>
		/// <returns>The vector3.</returns>
		/// <param name="v">V.</param>
		Vector3 RoundVector3 (Vector3 v)
		{
			return new Vector3 (Mathf.Round (v.x), Mathf.Round (v.y), Mathf.Round (v.z));
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


