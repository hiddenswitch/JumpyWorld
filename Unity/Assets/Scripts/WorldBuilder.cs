using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class WorldBuilder : MonoBehaviour
	{
        public int seed;

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
        public float connectLooseDistance;

        struct WorldBuilderInfo
        {
            public Anchor anchor;
            public Generator generator;
        }
		// Use this for initialization
		public void Start ()
		{
            var oldSeed = Random.seed;
            Random.seed = seed;
            List<Anchor> pendingAnchors = new List<Anchor>();
            List<WorldBuilderInfo> pendingInfos = new List<WorldBuilderInfo>();
            startRoom.Build();
            pendingAnchors.AddRange(startRoom.anchors);
            Debug.Log(startRoom.anchors.Length);
            List<Anchor> looseAnchors = new List<Anchor>();

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
                            x -= Random.Range(0, RoomShift);// * (int) anchor.directions.ToVector().z;
						} else {
                            y -= Random.Range(0, RoomShift);// * (int) anchor.directions.ToVector().x;
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
                            Destroy(anchor.generator.gameObject);
                            //looseAnchors.Add(anchor);
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
                                looseAnchors.Add(anchor);
								break;
							}
                            newAnchors.Add(newHallway.anchors[1]);  //Hardcoding this for now.
                            
                        } else
                        {
                            looseAnchors.Add(anchor);
                        }
                    }


                    pendingAnchors = newAnchors;
                }
                roomGeneratingIteration = !roomGeneratingIteration;
            }

            looseAnchors.AddRange(pendingAnchors);

            HashSet<Anchor> filledAnchors = new HashSet<Anchor>();
            foreach(Anchor a in looseAnchors)
            {
                foreach(Anchor b in looseAnchors)
                {
                    if (filledAnchors.Contains(a) || filledAnchors.Contains(b))
                    {
                        continue;
                    }
                    if (shouldGenerate(a, b))
                    {
                        generateHallway(a.position, b.position, true);
                        filledAnchors.Add(a);
                        filledAnchors.Add(b);
                    }
                }
            }

            Random.seed = oldSeed;
        }
	
		// Update is called once per frame
		void Update ()
		{
	
		}

        Anchor findCloestAnchorOfDifferentParent(Anchor a, List<Anchor> all)
        {
            Anchor cloest = all[0];
            float d = a.generator != cloest.generator ? Vector3.Distance(cloest.position, a.position) : float.MaxValue;
            foreach (Anchor b in all)
            {
                if (b.generator == a.generator)
                {
                   
                    continue;
                }
                float dNew = Vector3.Distance(a.position, b.position);
                if (dNew < d)
                {
                    dNew = d;
                    cloest = b;
                }
            }
            return cloest;
        }

        bool shouldGenerate(Anchor a, Anchor b)
        {
            bool angleCheck = Vector3.Dot((b.position - a.position).normalized, a.directions.ToVector()) > 0.5 && Vector3.Dot((a.position - b.position).normalized, b.directions.ToVector()) > 0.5;
            bool distanceCheck = Vector3.Distance(b.position, a.position) < connectLooseDistance ;
            bool parentCheck = a.generator != b.generator;
            return angleCheck && distanceCheck && parentCheck;
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


        Hallway generateHallway(Vector3 startPoint, Vector3 endPoint, bool ignoreCollision=false)
        {
            GameObject hallwayObj = Instantiate(hallwayPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			tileDrawer.parent = hallwayObj.transform;
            Hallway hallway = hallwayObj.GetComponent<Hallway>();

            hallway.startPoint = startPoint;
            hallway.endPoint = endPoint;

			tileDrawer.startCollisionTest ();
            hallway.Build();
			if (tileDrawer.getCollisionTestResult () && !ignoreCollision) {
				Destroy (hallwayObj);
				return null;
			}
            return hallway;
        }


	}
}