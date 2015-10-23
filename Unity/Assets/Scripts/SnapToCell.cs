using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class SnapToCell : MonoBehaviour
	{
		public float cellSize;
		public int smoothFrames;
		/// <summary>
		/// When true, starts a smoothing snap; (then set to false.)
		/// </summary>
		[Header("Runtime")]
		bool
			shouldSnapX;
		bool shouldSnapZ;
		/// <summary>
		/// When true, quits any existing smoothing snaps.
		/// </summary>
		bool quitSmoothX;
		bool quitSmoothZ;
		/// <summary>
		/// When true, there is a smoothing snap running.
		/// </summary>
		bool smoothingX = false;
		bool smoothingZ = false;

		void Start ()
		{
			
		}
		
		void Update ()
		{
			if (shouldSnapX) {
				SnapX ();
			}
			if (shouldSnapZ) {
				SnapZ ();
			}
		}

		void SnapX ()
		{
			if (!smoothingX) {
				StartCoroutine (SmoothX ());
			}
			shouldSnapX = false;

		}

		void SnapZ ()
		{
			if (!smoothingZ) {
				StartCoroutine (SmoothZ ());
			}
			shouldSnapZ = false;
		}

		IEnumerator SmoothX ()
		{
			smoothingX = true;
			for (int i = 0; (i < smoothFrames); i++) {
				Vector3 oldPosition = transform.position;
				Vector3 newPosition = new Vector3 (ComputeNearestCell (oldPosition.x), oldPosition.y, oldPosition.z); 
				transform.position = Vector3.Lerp (oldPosition, newPosition, i / (float)smoothFrames);

				if (!quitSmoothX) {
					yield return new WaitForFixedUpdate ();
				} else {
					quitSmoothX = false;
					transform.position = newPosition;
					i = smoothFrames;
				}
			}
			smoothingX = false;
		}

		IEnumerator SmoothZ ()
		{
			smoothingZ = true;
			for (int i = 0; (i < smoothFrames); i++) {
				Vector3 oldPosition = transform.position;
				Vector3 newPosition = new Vector3 (oldPosition.x, oldPosition.y, ComputeNearestCell (oldPosition.z)); 
				transform.position = Vector3.Lerp (oldPosition, newPosition, i / (float)smoothFrames);

				if (!quitSmoothZ) {
					yield return new WaitForFixedUpdate ();
				} else {
					quitSmoothZ = false;
					transform.position = newPosition;
					i = smoothFrames;
				}
			}
			smoothingZ = false;
		}

		void TurnDirection (Vector3 targetRotation)
		{
			if (!enabled) {
				return;
			}
			if (targetRotation.y == 0f || targetRotation.y == 180f) {
				shouldSnapX = true;
				quitSmoothX = false;
				quitSmoothZ = true;
			} else if (targetRotation.y == 90f || targetRotation.y == -90f) {
				shouldSnapZ = true;
				quitSmoothX = true;
				quitSmoothZ = false;
			} else {
				Debug.LogError ("there is a problem with targetRotation with snapToCell that requires fixing.");
			}
		}

		/// <summary>
		/// Computes the nearest cell to the current position. Assumes that the cells are gridded, with (0,0) being a valid center.
		/// </summary>
		/// <returns>The nearest cell.</returns>
		/// <param name="currentPos">Current position.</param>
		float ComputeNearestCell (float currentPos)
		{
			return Mathf.Round (currentPos / cellSize) * cellSize;
		}
	}
}
