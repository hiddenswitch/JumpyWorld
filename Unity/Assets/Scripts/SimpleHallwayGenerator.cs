using UnityEngine;
using System.Collections;
using System;
namespace JumpyWorld
{
    public class SimpleHallwayGenerator : HallwayGenerator
    {

        public GameObject cube;
        public GameObject gateWay;

        public override void generateHallway(Vector3 startPosition, Vector3 endPosition)
        {
            Vector3 currentPos = startPosition;
            //incorrect / sloppy code written as fast as possible. We are not using this anyway.
            while (currentPos.x < endPosition.x)
            {
                Instantiate(cube, currentPos, Quaternion.identity);
                currentPos += new Vector3(1, 0, 0);
            }
            while (currentPos.x > endPosition.x)
            {
                Instantiate(cube, currentPos, Quaternion.identity);
                currentPos -= new Vector3(1, 0, 0);
            }
            while (currentPos.z < endPosition.z)
            {
                Instantiate(cube, currentPos, Quaternion.identity);
                currentPos += new Vector3(0, 0, 1);
            }
            while (currentPos.z > endPosition.z)
            {
                Instantiate(cube, currentPos, Quaternion.identity);
                currentPos -= new Vector3(0, 0, 1);
            }
            GameObject g = Instantiate(gateWay, endPosition, Quaternion.identity) as GameObject;
            Debug.Log(startPosition);
            Debug.Log(endPosition);
            g.GetComponent<GenerateRoomOnCollision>().roomCenter = (endPosition - startPosition).normalized * 2f + endPosition;
            Debug.Log(gateWay.GetComponent<GenerateRoomOnCollision>().roomCenter);
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
