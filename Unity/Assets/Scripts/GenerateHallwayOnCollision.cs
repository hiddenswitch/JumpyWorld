using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    public class GenerateHallwayOnCollision : MonoBehaviour
    {
        public HallwayGenerator hallwayGenerator;
        public Vector3 startPos;
        public Vector3 endPos;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void generatePlatform(PlatformConnection owner)
        {
            Platform p = hallwayGenerator.generateHallway(startPos, endPos);
            owner.setOtherPlatform(p);
        }
    }
}
