using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class RigidBodyMoveController : MonoBehaviour
	{
        public Rigidbody rigidBody;
        private CapsuleCollider capsule;

		public float speed = 0.85f;

        //might not be used
        //public float groundCheckDistance = 0.01f;

        [HideInInspector]
		public bool moving = true;
		public bool isAlive = true;
        public float currentTargetSpeed =3f;

        public Vector3 Velocity
        {
            get { return rigidBody.velocity; }
        }

        public void getTargetSpeed()
        {
            if (moving && isAlive)
            {
                currentTargetSpeed = speed;

            }
            else
            {
                currentTargetSpeed = 0;
            }
        }


        // Use this for initialization
        void Start ()
		{
            // Move controller is disabled for non-local players.
            /*
			if (!isLocalPlayer) {
				enabled = false;
				return;
			}*/
            rigidBody = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();

        }

        void Update()
        {

        }




		
		void HandleOnGameOver ()
		{
			enabled = false;
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
            getTargetSpeed();
            Vector3 desiredMove = transform.forward.normalized * currentTargetSpeed;
            if (onGround())
            {
                rigidBody.velocity = desiredMove;

            }
            //else, fall.
        }

        void TurnDirection(Vector3 targetRotation)
        {
            //transform.rotation = Quaternion.Euler(targetRotation);
            StartCoroutine(smoothTurn(targetRotation));
        }

        private bool onGround()
        {
            RaycastHit hitInfo;

            /** this somehow doesn't work, I don't know why.
            if (Physics.SphereCast(transform.position,0.05f, -1 * transform.up, out hitInfo,
                                   ((capsule.height / 2f) - capsule.radius) + 0.1f))
            {
                Debug.Log("hit");
                Debug.Log(hitInfo.transform.position);
                return true;
            }
            else{

                return false;
            }
            **/
            Vector3 ray1Origin = transform.position;
            Vector3 ray2Origin = transform.position;

            ray1Origin.x += 0.1f;
            ray1Origin.z += 0.1f;
            ray1Origin.y += 1.0f;
            ray2Origin.y += 1.0f;
            ray2Origin.x -= 0.1f;
            ray2Origin.z -= 0.1f;

            bool ray1Hit = Physics.Raycast(ray1Origin, Vector3.down, out hitInfo, ((capsule.height / 2f) - capsule.radius) + 1.1f);
            bool ray2Hit = Physics.Raycast(ray2Origin, Vector3.down, out hitInfo, ((capsule.height / 2f) - capsule.radius) + 1.1f);
            return ray1Hit || ray2Hit;

        }


        IEnumerator smoothTurn(Vector3 targetRotationEuler)
        {
            Quaternion startRotation = transform.rotation;
            for (int i = 0; i <5; i++)
            {
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(targetRotationEuler), i / (float)4);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}