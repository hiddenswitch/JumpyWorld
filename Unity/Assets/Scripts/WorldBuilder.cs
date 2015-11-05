using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class WorldBuilder : MonoBehaviour
	{
		public TileDrawer tileDrawer;
		public TilePool tilePool;

        public GameObject roomPrefab;
        public GameObject hallwayPrefab;

        public Room startRoom;
        public List<Room> rooms = new List<Room>();
        public int iterations = 2;

        bool roomGeneratingIteration = false;

        public int minHallwayLength = 10;
        public int maxHallwayLength = 20;

        public int minRoomDimension = 5;
        public int maxRoomDimension = 8;

        public float newHallwayProbablity = 0.4f;

		// Use this for initialization
		void Start ()
		{
            List<Anchor> pendingAnchors = new List<Anchor>();
            startRoom.Build();
            pendingAnchors.AddRange(startRoom.anchors);
            Debug.Log(startRoom.anchors.Length);
            for (int i = 0; i < iterations; i++)
            {
                List<Anchor> newAnchors = new List<Anchor>();
                if (roomGeneratingIteration)
                {
                    foreach(Anchor anchor in pendingAnchors)
                    {
                        int w = Random.Range(minRoomDimension, maxRoomDimension);
                        int h = Random.Range(minRoomDimension, maxRoomDimension);


                        int x = (int)anchor.position.x; //+ (int) ( anchor.directions.ToVector().x) * w;
                        int y = (int)anchor.position.z; //+ (int)(anchor.directions.ToVector().z) * h;

                        Room newRoom = generateRoom(new Rect(x, y, w, h));


                        foreach (Anchor newAnchor in newRoom.anchors)
                        {
                            if (newAnchor.directions.ToVector() != - anchor.directions.ToVector())
                            {
                                newAnchors.Add(newAnchor);
                            }
                        }

                    }
                    pendingAnchors = newAnchors;
                } else
                {
                    foreach (Anchor anchor in pendingAnchors)
                    {
                        if (Random.value < newHallwayProbablity) { 
                            Vector3 endPoint = Random.Range(minHallwayLength, maxHallwayLength) * anchor.directions.ToVector() + anchor.position;
                            Hallway newHallway = generateHallway(anchor.position, endPoint);
                            newAnchors.Add(newHallway.anchors[1]);  //Hardcoding this for now.
                        }
                    }

                    pendingAnchors = newAnchors;
                }
                roomGeneratingIteration = !roomGeneratingIteration;

            }


        }
	
		// Update is called once per frame
		void Update ()
		{
	
		}


        Room generateRoom (Rect options)
        {
            GameObject roomObj = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            Room room = roomObj.GetComponent<Room>();

            room.size = options;

            room.Build();
            return room;


        }


        Hallway generateHallway(Vector3 startPoint, Vector3 endPoint)
        {
            GameObject hallwayObj = Instantiate(hallwayPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            Hallway hallway = hallwayObj.GetComponent<Hallway>();

            hallway.startPoint = startPoint;
            hallway.endPoint = endPoint;

            hallway.Build();
            return hallway;
        }
	}
}