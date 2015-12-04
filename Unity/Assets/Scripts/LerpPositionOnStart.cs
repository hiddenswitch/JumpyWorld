using UnityEngine;
using System.Collections;

namespace JumpyWorld { 
    public class LerpPositionOnStart : MonoBehaviour {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public AnimationCurve lerpCurve;
        public float lerpDuration;
        public bool useLocalPosition;


	    // Use this for initialization
	    void Start () {
	        if (useLocalPosition)
            {
                transform.localPosition = startPosition;
            } else
            {
                transform.position = startPosition;
            }
            StartCoroutine(lerpPositionCoroutine());
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        IEnumerator lerpPositionCoroutine()
        {
            float startTime = Time.time;
            while (Time.time - startTime <= lerpDuration)
            {
                Vector3 targetPosition = Vector3.LerpUnclamped(startPosition, endPosition, lerpCurve.Evaluate((Time.time - startTime) / lerpDuration));
                if (useLocalPosition)
                {
                    transform.localPosition = targetPosition;
                }
                else
                {
                    transform.position = targetPosition;
                }
                yield return new WaitForEndOfFrame();
            }
            if (useLocalPosition)
            {
                transform.localPosition = endPosition;
            }
            else
            {
                transform.position = endPosition;
            }
        }
    }
}