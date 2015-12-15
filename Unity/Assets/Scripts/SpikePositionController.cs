using UnityEngine;
using System.Collections;


namespace JumpyWorld
{
    public class SpikePositionController : MonoBehaviour
    {

        public SpikeStateManager ssm;
        public Vector3 retractedLocalPosition;
        public Vector3 extendedLocalPosition;
        public Collider blockingMovementCollider;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(ssm.currentState == SpikeStateManager.SpikeStates.Extending)
            {
                gameObject.transform.localPosition = Vector3.Lerp(retractedLocalPosition, extendedLocalPosition, 1 - ssm.timeRemainingInCurrentState / ssm.spikeStateTimes[SpikeStateManager.SpikeStates.Extending]);
            } else if (ssm.currentState == SpikeStateManager.SpikeStates.Retracting)
            {
                gameObject.transform.localPosition = Vector3.Lerp(extendedLocalPosition, retractedLocalPosition, 1 - ssm.timeRemainingInCurrentState / ssm.spikeStateTimes[SpikeStateManager.SpikeStates.Retracting]);
            }

            if (ssm.currentState == SpikeStateManager.SpikeStates.Extended || ssm.currentState == SpikeStateManager.SpikeStates.Retracting)
            {
                blockingMovementCollider.enabled = true;
            } else
            {
                blockingMovementCollider.enabled = false;
            }



        }
    }
}