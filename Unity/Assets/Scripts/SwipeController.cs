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
		
		
		// Use this for initialization
		void Start ()
		{
			// Disable the controller for non-local player objects
			/*
			if (!isLocalPlayer) {
				enabled = false;
				return;
			}*/
			
			screenSpaceNorth = screenSpaceNorth.normalized;
			screenSpaceEast = screenSpaceEast.normalized;
			
			// TODO: Put into editor
			/*
			screenSpaceEast = (new Vector2 (271.2969f, 344.0117f) - new Vector2 (255.5f, 337.5195f)).normalized;
			screenSpaceNorth = (new Vector2 (272.5234f, 345.7578f) - new Vector2 (285.5039f, 333.2734f)).normalized;
			*/
		}
		
		
		
		// Update is called once per frame
		void Update ()
		{
			Vector2 touchPosition = Vector2.zero;
			
			if (Application.isMobilePlatform) {
				if (Input.touchCount == 0) {
					return;
				}
				
				var touch = Input.GetTouch (0);
				touchPosition = touch.position;
				
				if (touch.phase == TouchPhase.Began) {
					touchDown = touchPosition;
				}
			} else {
				var down = Input.GetMouseButton (0);
				
				if (down) {
					if (!prevFrameDown) {
						prevFrameDown = true;
						touchDown = Input.mousePosition;
					}
					touchPosition = Input.mousePosition;
				} else {
					prevFrameDown = false;
					return;
				}
			}
			
			swipe = touchPosition - touchDown;
			distanceTraveled = swipe.magnitude;
			
			if (distanceTraveled < minSwipeDistance) {
				return;
			}
			
			swipe = swipe.normalized;
			
			var northness = Vector2.Dot (swipe, screenSpaceNorth);
			if (northness > tolerance) {
				BroadcastMessage (swipeNorth);
				touchDown = touchPosition;
				return;
			}
			
			if (northness < -tolerance) {
				// South
				BroadcastMessage (swipeSouth);
				touchDown = touchPosition;
				return;
			}
			
			var eastness = Vector2.Dot (swipe, screenSpaceEast);
			if (eastness > tolerance) {
				BroadcastMessage (swipeEast);
				touchDown = touchPosition;
				return;
			}
			
			if (eastness < -tolerance) {
				// West
				BroadcastMessage (swipeWest);
				touchDown = touchPosition;
				return;
			}
		}
	}
}
