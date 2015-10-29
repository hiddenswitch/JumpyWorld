using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    [RequireComponent(typeof (GenerateHallwayOnCollision))]
    public class SimpleRoomGatewaySetup : MonoBehaviour
    {
        public Vector3 hallwayTargetOffset;
        // Use this for initialization
        void Start()
        {
            GenerateHallwayOnCollision g = GetComponent<GenerateHallwayOnCollision>();
            g.endPos = transform.position + hallwayTargetOffset;
            g.startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
