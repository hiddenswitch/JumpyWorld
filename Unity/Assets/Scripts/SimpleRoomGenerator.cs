using UnityEngine;
using System.Collections;
using System;
namespace JumpyWorld
{
    public class SimpleRoomGenerator : RoomGenerator
    {
        public GameObject simpleRoomPrefab;

        public override Platform generateRoom(Vector3 roomCenterPosition, int xSize, int ySize)
        {
            GameObject simpleRoom = Instantiate(simpleRoomPrefab, roomCenterPosition, Quaternion.identity) as GameObject;
            return simpleRoom.GetComponent<Platform>();

            /**
            The simple room generator does not do things correctly. Below are certain ways to make it better, but since we are not going to use it on the long run anyway,
            I decided to drop actrually writing this code.
            Vector3 currentScale = simpleRoom.transform.localScale;
            simpleRoom.transform.localScale = new Vector3(currentScale.x * xSize, currentScale.y * ySize, 1);

            //Add gateways at correct positions;
            //Add walls

            **/

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
