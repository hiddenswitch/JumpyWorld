using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    public class GenerateRoomOnCollision : MonoBehaviour
    {
        public RoomGenerator roomGenerator;
        public Vector3 roomCenter;
        public int roomXSize = 5;
        public int roomYSize = 5;

        private Vector3 previous;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (roomCenter != previous)
            {
                Debug.Log(roomCenter);
                previous = roomCenter;
            }
        }


        void generatePlatform(PlatformConnection owner)
        {
            Platform p = roomGenerator.generateRoom(roomCenter, roomXSize, roomYSize);
            owner.setOtherPlatform(p);
        }
    }
}
