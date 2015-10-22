using UnityEngine;
using System.Collections;

namespace JumpyWorld{
	public class SnapToCell : MonoBehaviour {
		public float cellSize;
		public int smoothFrames;

		[Header("Runtime")]
		bool shouldSnapX; //when true, starts a smoothing snap; (then set to false.)
		bool shouldSnapZ;
		bool quitSmoothX; //when true, quits any existing smoothing snaps.
		bool quitSmoothZ;

		bool smoothingX = false; //when true, there is a smoothing snap running.
		bool smoothingZ = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (shouldSnapX) {
				snapX();
			}
			if (shouldSnapZ) {
				snapZ();
			}
		}

		void snapX () {
			if (!smoothingX) {
				StartCoroutine (smoothX ());
			}
			shouldSnapX = false;

		}

		void snapZ () {
			if (!smoothingZ) {
				StartCoroutine (smoothZ ());
			}
			shouldSnapZ = false;
		}

		IEnumerator smoothX(){
			smoothingX = true;
			for (int i = 0; (i < smoothFrames) ; i++) {
				Vector3 oldPosition = transform.position;
				Vector3 newPosition = new Vector3 (computeNearestCell(oldPosition.x), oldPosition.y, oldPosition.z); 
				transform.position = Vector3.Lerp (oldPosition, newPosition, i / (float) smoothFrames);

				if (!quitSmoothX){
					yield return new WaitForFixedUpdate();
				} else {
					quitSmoothX = false;
					transform.position = newPosition;
					i = smoothFrames;
				}
			}
			smoothingX = false;
		}

		IEnumerator smoothZ(){
			smoothingZ = true;
			for (int i = 0; (i < smoothFrames) ; i++) {
				Vector3 oldPosition = transform.position;
				Vector3 newPosition = new Vector3 (oldPosition.x, oldPosition.y,computeNearestCell(oldPosition.z)); 
				transform.position = Vector3.Lerp (oldPosition, newPosition, i / (float) smoothFrames);

				if (!quitSmoothZ){
					yield return new WaitForFixedUpdate();
				} else {
					quitSmoothZ = false;
					transform.position = newPosition;
					i = smoothFrames;
				}
			}
			smoothingZ = false;
		}


        void TurnDirection(Vector3 targetRotation)
        {
            if (!enabled)
            {
                return;
            }
            if (targetRotation.y == 0f || targetRotation.y == 180f)
            {
                shouldSnapX = true;
                quitSmoothX = false;
                quitSmoothZ = true;
            } else if (targetRotation.y == 90f || targetRotation.y == -90f)
            {
                shouldSnapZ = true;
                quitSmoothX = true;
                quitSmoothZ = false;
            } else
            {
                Debug.LogError("there is a problem with targetRotation with snapToCell that requires fixing.");
            }
        }



		//Assumes that the cells are gridded, with (0,0) being a valid center.
		float computeNearestCell(float currentPos){
			return Mathf.Round (currentPos / cellSize) * cellSize;
		}
	}
}
