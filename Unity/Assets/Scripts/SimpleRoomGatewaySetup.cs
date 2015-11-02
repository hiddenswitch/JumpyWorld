using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    [RequireComponent(typeof (GenerateHallwayOnCollision))]
    [RequireComponent(typeof (PlatformConnection))]
    public class SimpleRoomGatewaySetup : MonoBehaviour
    {
        public Vector3 hallwayTargetOffset;
        public Platform simpleRoom;
        // Use this for initialization
        void Start()
        {
            GenerateHallwayOnCollision g = GetComponent<GenerateHallwayOnCollision>();
            g.endPos = transform.position + hallwayTargetOffset;
            g.startPos = transform.position;
            PlatformConnection pc = GetComponent<PlatformConnection>();
            pc.setOwnerPlatform(simpleRoom);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
