using UnityEngine;
using System.Collections;
namespace JumpyWorld { 
    [RequireComponent (typeof (Rigidbody))]
    public class PortalAnimation : MonoBehaviour {


        public float elevationTime;
        public float fallTime;
        public AnimationCurve elevationVelocityCurve;
        public AnimationCurve fallVelocityCurve;
        public ClearAndRebuildWorld rebuild;
        Rigidbody rb;

	    // Use this for initialization
	    void Start () {
            rb = GetComponent<Rigidbody>();
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        void StartPortalAnimation()
        {
            StartCoroutine(PortalAnimationCoroutine());
        }

        IEnumerator PortalAnimationCoroutine()
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            float time = Time.time;
            while (Time.time - time < elevationTime)
            {

                gameObject.transform.position += elevationVelocityCurve.Evaluate((Time.time - time)/ elevationTime) * gameObject.transform.up * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            rebuild.RandomizeAndRebuild();
            BroadcastMessage("ResetPosition");
            time = Time.time;


            while (Time.time - time < fallTime)
            {
                gameObject.transform.position += fallVelocityCurve.Evaluate((Time.time - time) / fallTime) * gameObject.transform.up * Time.deltaTime * -1;
                yield return new WaitForEndOfFrame();
            }

            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}