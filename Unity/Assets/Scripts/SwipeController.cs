using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class SwipeController : MonoBehaviour
	{
		[Header("Options")]
		public float
			minSwipeDistance = 5f;
		public float tolerance = 0.906f;
		public Vector2 screenSpaceNorth = Vector2.up;
		public Vector2 screenSpaceEast = Vector2.right;
		public Vector3 worldSpaceUp = Vector3.up;
		[Header("Events")]
		public string
			swipeNorth = "SwipeNorth";
		public string swipeSouth = "SwipeSouth";
		public string swipeEast = "SwipeEast";
		public string swipeWest = "SwipeWest";
		[Header("Runtime")]
		public Vector2
			touchDown;
		public Vector2 swipe;
		public bool prevFrameDown;
		public float distanceTraveled;
		private bool directionSet;
		public Vector2 lastPosition;
        
		// Use this for initialization
		void Start ()
		{
			screenSpaceNorth = screenSpaceNorth.normalized;
			screenSpaceEast = screenSpaceEast.normalized;
		}
        
		// Update is called once per frame
		void Update ()
		{
			Vector2 touchPosition = Vector2.zero;
            
			bool down;

			if (Input.touchSupported) {
				down = Input.touchCount > 0;
				lastPosition = Input.touchCount > 0 ? Input.GetTouch (0).position : Vector2.zero;
			} else {
				down = Input.GetMouseButton (0);
				lastPosition = Input.mousePosition;
			}

			// Is the mouse / touch down on this frame?
			if (down) {
				// Is this the first touch?
				if (!prevFrameDown) {
					prevFrameDown = true;
					touchDown = lastPosition;
				}

				touchPosition = lastPosition;

				swipe = touchPosition - touchDown;
				distanceTraveled = swipe.magnitude;
				
				// This is the sensitivity constraint.
				if (distanceTraveled < minSwipeDistance) {
					return;
				}
				
				// Did we already choose a direction during this swipe?
				if (directionSet) {
					return;
				}
				
				
				touchDown = touchPosition;
				directionSet = true;
				
				SetDirectionForSwipe (swipe);
			} else {
				// If we just lifted up the touch / click and we set a direction, change the direction in case
				// the final direction is different
				if (prevFrameDown
					&& directionSet) {
					// directionSet will only be true if the sensitivity constraint has already been met,
					// so it's redundant to check sensitivity here again
					SetDirectionForSwipe (lastPosition - touchDown);
				}

				// Now, officially register a new swipe
				prevFrameDown = false;
				directionSet = false;
			}
		}

		void SetDirectionForSwipe (Vector3 swipe)
		{
			swipe = swipe.normalized;

			var northness = Vector2.Dot (swipe, screenSpaceNorth);
			var eastness = Vector2.Dot (swipe, screenSpaceEast);
			
			if (northness > tolerance) {
				BroadcastMessage (swipeNorth);
			} else if (northness < -tolerance) {
				// South
				BroadcastMessage (swipeSouth);
			} else if (eastness > tolerance) {
				BroadcastMessage (swipeEast);
			} else if (eastness < -tolerance) {
				// West
				BroadcastMessage (swipeWest);
			}
		}
	}
}
