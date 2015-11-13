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
        public int iterations;

        bool roomGeneratingIteration = false;

        public int minHallwayLength;
        public int maxHallwayLength ;


        public int minRoomDimension ;
        public int maxRoomDimension;


        public float connectLooseDistance;

		[Header("Hallways")]
		public float newHallwayProbability;
		public AnimationCurve hallwayTurbulenceDistribution;

		[Header("Decorations")]
		public RandomPlacerForRoom treeDecorator;

        private class WorldBuilderInfo
        {
            public Anchor anchor;
			public WorldBuilderInfo parent;
			public WorldBuilderInfo(Anchor anchor, WorldBuilderInfo info){
				this.anchor = anchor;
				this.parent = info;
			}

        }
		// Use this for initialization
		public void Start ()
		{
            var oldSeed = Random.seed;
            Random.seed = seed;
            List<WorldBuilderInfo> pendingInfos = new List<WorldBuilderInfo>();



            startRoom.Build();

			foreach (Anchor anchor in startRoom.anchors) {
				pendingInfos.Add (new WorldBuilderInfo(anchor, null));
			}

			List<Anchor> looseAnchors = new List<Anchor>();

            for (int i = 0; i < iterations; i++)
            {
				List<WorldBuilderInfo> newInfo = new List<WorldBuilderInfo>();

				foreach (WorldBuilderInfo info in pendingInfos){
					Anchor anchor = info.anchor;
					if (anchor.position.x == 9 && anchor.position.y == 0 && anchor.position.z == -26){
						Debug.Log ("hi");
					}

					if (roomGeneratingIteration)
					{
						int w = Random.Range(minRoomDimension, maxRoomDimension);
						int h = Random.Range(minRoomDimension, maxRoomDimension);
						
						
						int x = (int)anchor.position.x + Mathf.Min (0,(int)(anchor.directions.ToVector().x)) * w;
						int y = (int)anchor.position.z + Mathf.Min (0,(int)(anchor.directions.ToVector().z)) * h;
						
						
						if (anchor.directions.ToVector().x == 0){
							x -= Random.Range(1, w - 1);// * (int) anchor.directions.ToVector().z;
						} else {
							y -= Random.Range(1, h - 1);// * (int) anchor.directions.ToVector().x;
						}
						
						Room newRoom = generateRoom(new Rect(x, y, w, h));
						
						if (newRoom != null){
							
							foreach (Anchor newAnchor in newRoom.anchors)
							{

								if (newAnchor.directions.ToVector() != - anchor.directions.ToVector())
								{

									newInfo.Add(new WorldBuilderInfo(newAnchor, info));
								}
							}
						} else {
							looseAnchors.Add(info.parent.anchor);
							Debug.Log (info.parent.anchor.position);
							Destroy(anchor.generator.gameObject);
						}

					} else {
						if (Random.value < newHallwayProbability) { 
							Vector3 endPoint = Random.Range(minHallwayLength, maxHallwayLength) * anchor.directions.ToVector() + anchor.position;
							Hallway newHallway = generateHallway(anchor.position, endPoint);
							if (newHallway == null){
								looseAnchors.Add(anchor);
								Debug.Log (anchor.position);
								continue;
							}
							newInfo.Add(new WorldBuilderInfo(newHallway.anchors[1], info));  //Hardcoding this for now.
							
						} else {
							looseAnchors.Add (anchor);
						}

					}

				}
				pendingInfos = newInfo;

				foreach (WorldBuilderInfo info in pendingInfos){
					Anchor anchor = info.anchor;

				}


				roomGeneratingIteration = !roomGeneratingIteration;
            }
			Debug.Log (pendingInfos.Count);
			Debug.Log (looseAnchors.Count);
            foreach (WorldBuilderInfo info in pendingInfos) {
				Anchor anchor = info.anchor;
				looseAnchors.Add(info.anchor);
			}

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

			foreach (Anchor a in looseAnchors) {
				if (!filledAnchors.Contains(a) && a.generator is Hallway){
					Destroy (a.generator.gameObject);
				}
			}

            Random.seed = oldSeed;
        }
	
		// Update is called once per frame
		void Update ()
		{
	
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

			// Generate trees
			treeDecorator.room = room;
			treeDecorator.Generate(seed: Random.Range(0, 65536));
			treeDecorator.Draw(tileDrawer: tileDrawer);
            return room;


        }


        Hallway generateHallway(Vector3 startPoint, Vector3 endPoint, bool ignoreCollision=false)
        {
            GameObject hallwayObj = Instantiate(hallwayPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			tileDrawer.parent = hallwayObj.transform;
            Hallway hallway = hallwayObj.GetComponent<Hallway>();

            hallway.startPoint = startPoint;
            hallway.endPoint = endPoint;
			hallway.turbulence = hallwayTurbulenceDistribution.Evaluate(Random.value);
			hallway.shouldBalanceCorners = true;
			hallway.seed = Random.Range (0, 65535);

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