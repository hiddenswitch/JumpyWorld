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
		private List<Vector3> pathP = new List<Vector3>();
		private Vector3 currentPt;

		// Use this for initialization
		void Start ()
		{
			// for testing purposes only!
			target.gameObject.BroadcastMessage ("DelayedStart");
			currentPt = this.gameObject.transform.position;
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
			pathP.Clear();
			var arrivedThreshold = moveSpeed / 2 * Time.fixedDeltaTime;
			var delayedController = this.gameObject.GetComponent<DelayedTurnController> ();
			var snapThreshold = Mathf.Min (Mathf.Abs(delayedController.enterCellThreshold), Mathf.Abs(delayedController.exitCellThreshold));


			var myTransform = this.gameObject.transform;
			var distance = Vector3.Distance (myTransform.position, target.position);
			var bounds = floor.size;

			var targetInBounds = (bounds.xMin <= target.position.x) && 
				(target.position.x <= bounds.xMax) && 
					(bounds.yMin <= target.position.z) && 
					(target.position.z <= bounds.yMax);
			
			// only update if the target is in bounds and not too close
			if (targetInBounds && distance > stop) {
				var path = Hallway.BresenhamFilled (
					from: RoundVector3(myTransform.position),
					to: RoundVector3(target.position),
					shouldBalanceCorners: true);
				
				pathP = new List<Vector3> (path);

				foreach (var point in path) {
					if (point != RoundVector3(myTransform.position)) {
						
						Vector3 currentDirection = myTransform.forward.normalized;
						Vector3 direction = currentDirection;
						if (CloseEnough(myTransform.position, RoundVector3(myTransform.position), snapThreshold)) {
							direction = (point - RoundVector3(myTransform.position)).normalized;
						}
						
						if(currentDirection != direction) {
							if (direction == Vector3.left) {
								myTransform.gameObject.BroadcastMessage("SwipeWest");
							}
							if (direction == Vector3.right) {
								myTransform.gameObject.BroadcastMessage("SwipeEast");
							}
							if (direction == Vector3.forward) {
								myTransform.gameObject.BroadcastMessage("SwipeNorth");
							}
							if (direction == Vector3.back) {
								myTransform.gameObject.BroadcastMessage("SwipeSouth");
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
			else if ((!CloseEnough(currentPt, myTransform.position, arrivedThreshold)) && distance > stop) {
				Vector3 newDirection = RoundVector3((currentPt - myTransform.position).normalized);
				if (newDirection == Vector3.left || newDirection == Vector3.right || newDirection == Vector3.forward || newDirection == Vector3.back) {
					myTransform.forward = newDirection;
					var velocity = myTransform.forward * moveSpeed;
					myTransform.position = myTransform.position + velocity * Time.deltaTime;
				}
			}
		}

		void OnDrawGizmos() {
			for (var i = 0; i < pathP.Count - 1; i++) {
				Gizmos.DrawLine (pathP[i], pathP[i+1]);
			}
		}

		Vector3 RoundVector3(Vector3 v) {
			return new Vector3(Mathf.Round (v.x), Mathf.Round (v.y), Mathf.Round (v.z));
		}
		
		bool CloseEnough(Vector3 v1, Vector3 v2, float threshold) {
			return (v1 - v2).magnitude < threshold;
		}

	}
}


