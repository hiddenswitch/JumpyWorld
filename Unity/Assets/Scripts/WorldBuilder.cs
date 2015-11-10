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

        public int minHallwayLength = 8;
        public int maxHallwayLength = 11;

		public int HallwayShift = 5;		

        public int minRoomDimension = 12;
        public int maxRoomDimension = 15;

		public int RoomShift = 5;

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


                        int x = (int)anchor.position.x + Mathf.Min (0,(int)(anchor.directions.ToVector().x)) * w;
						int y = (int)anchor.position.z + Mathf.Min (0,(int)(anchor.directions.ToVector().z)) * h;


						if (anchor.directions.ToVector().x == 0){
							x += Random.Range (0, RoomShift);
						} else {
							y += Random.Range (0, RoomShift);
						}

                        Room newRoom = generateRoom(new Rect(x, y, w, h));



						if (newRoom != null){

	                        foreach (Anchor newAnchor in newRoom.anchors)
	                        {
	                            if (newAnchor.directions.ToVector() != - anchor.directions.ToVector())
	                            {
	                                newAnchors.Add(newAnchor);
	                            }
	                        }
						} else {
							//Destroy(anchor.Generator.gameObject)
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
							if (newHallway == null){
								break;
							}
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
			tileDrawer.parent = roomObj.transform;
            Room room = roomObj.GetComponent<Room>();

            room.size = options;


			tileDrawer.startCollisionTest ();
            room.Build();

			if (tileDrawer.getCollisionTestResult()) {
				Destroy(roomObj);
				return null;
			}
            return room;


        }


        Hallway generateHallway(Vector3 startPoint, Vector3 endPoint)
        {
            GameObject hallwayObj = Instantiate(hallwayPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			tileDrawer.parent = hallwayObj.transform;
            Hallway hallway = hallwayObj.GetComponent<Hallway>();

            hallway.startPoint = startPoint;
            hallway.endPoint = endPoint;

			tileDrawer.startCollisionTest ();
            hallway.Build();
			if (tileDrawer.getCollisionTestResult ()) {
				Destroy (hallwayObj);
				return null;
			}
            return hallway;
        }
	}
}