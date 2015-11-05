using UnityEngine;
using System.Collections;
using System;
namespace JumpyWorld
{
    public class SimpleHallwayGenerator : HallwayGenerator
    {

        public GameObject cube;
        public GameObject gateWay;

        public override Platform generateHallway(Vector3 startPosition, Vector3 endPosition)
        {
            GameObject hallway = new GameObject("simpleHallway");
            Platform hallwayP = hallway.AddComponent<Platform>();
            Vector3 currentPos = startPosition;
            //incorrect / sloppy code written as fast as possible. We are not using this anyway.
            while (currentPos.x < endPosition.x)
            {
                GameObject nxt = Instantiate(cube, currentPos, Quaternion.identity) as GameObject;
                nxt.transform.parent = hallway.transform;
                currentPos += new Vector3(1, 0, 0);
            }
            while (currentPos.x > endPosition.x)
            {
                GameObject nxt = Instantiate(cube, currentPos, Quaternion.identity) as GameObject;
                nxt.transform.parent = hallway.transform;
                currentPos -= new Vector3(1, 0, 0);
            }
            while (currentPos.z < endPosition.z)
            {
                GameObject nxt = Instantiate(cube, currentPos, Quaternion.identity) as GameObject;
                nxt.transform.parent = hallway.transform;
                currentPos += new Vector3(0, 0, 1);
            }
            while (currentPos.z > endPosition.z)
            {
                GameObject nxt = Instantiate(cube, currentPos, Quaternion.identity) as GameObject;
                nxt.transform.parent = hallway.transform;
                currentPos -= new Vector3(0, 0, 1);
            }
            GameObject g = Instantiate(gateWay, endPosition, Quaternion.identity) as GameObject;
            g.transform.parent = hallway.transform;
            g.GetComponent<GenerateRoomOnCollision>().roomCenter = (endPosition - startPosition).normalized * 2f + endPosition;
            g.GetComponent<PlatformConnection>().setOwnerPlatform(hallwayP);

            return hallwayP;
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
