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

		GameObject generatorParent;

        public GameObject roomPrefab;
        public GameObject hallwayPrefab;


        public Floor startRoom;
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

        [Header("Generators")]
        public List<Generator> generators;

        public IslandDepthPlacer depthGenerator;

		[Header("Treasure")]
		public RandomPlacerForRoom coinPlacer;


		[Header("Walls")]
		/// <summary>
		/// What is the probability that a room will have walls as a function
		/// of how far away from the center it is?
		/// </summary>
		public AnimationCurve wallProbability;

		List<Floor> floors = new List<Floor>();

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

			CreateNewGeneratorParent ();
            var oldSeed = Random.seed;
            Random.seed = seed;
            List<WorldBuilderInfo> pendingInfos = new List<WorldBuilderInfo>();
			floors.Clear();



            startRoom.Build();

			floors.Add(startRoom);

			foreach (Anchor anchor in startRoom.anchors) {
				pendingInfos.Add (new WorldBuilderInfo(anchor, null));
			}

			List<Anchor> looseAnchors = new List<Anchor>();

            for (int i = 0; i < iterations; i++)
            {
				List<WorldBuilderInfo> newInfo = new List<WorldBuilderInfo>();

				foreach (WorldBuilderInfo info in pendingInfos){
					Anchor anchor = info.anchor;

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
						
						Floor newRoom = GenerateFloor(new Rect(x, y, w, h));
						if (newRoom != null){
							floors.Add(newRoom);	
							foreach (Anchor newAnchor in newRoom.anchors)
							{

								if (newAnchor.directions.ToVector() != - anchor.directions.ToVector())
								{

									newInfo.Add(new WorldBuilderInfo(newAnchor, info));
								}
							}
						} else {
							looseAnchors.Add(info.parent.anchor);
							Destroy(anchor.generator.gameObject);
						}

					} else {
						if (Random.value < newHallwayProbability) { 
							Vector3 endPoint = Random.Range(minHallwayLength, maxHallwayLength) * anchor.directions.ToVector() + anchor.position;
							Hallway newHallway = GenerateHallway(anchor.position, endPoint);
							if (newHallway == null){
								looseAnchors.Add(anchor);
								continue;
							}
							newInfo.Add(new WorldBuilderInfo(newHallway.anchors[1], info));  //Hardcoding this for now.
							
						} else {
							looseAnchors.Add (anchor);
						}

					}

				}
				pendingInfos = newInfo;


				roomGeneratingIteration = !roomGeneratingIteration;
            }
            foreach (WorldBuilderInfo info in pendingInfos) {
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
                        GenerateHallway(a.position, b.position, true);
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
            AddDepth ();
			AddWalls();

            Random.seed = oldSeed;
        }
	
		void CreateNewGeneratorParent() {
			if (generatorParent != null) {
				Destroy(generatorParent);
				// TODO: Possibly destroy old objects that have been added to lists.
			}
			generatorParent = new GameObject("Generator Parent");
		}
		

        bool shouldGenerate(Anchor a, Anchor b)
        {
            bool angleCheck = Vector3.Dot((b.position - a.position).normalized, a.directions.ToVector()) > 0.5 && Vector3.Dot((a.position - b.position).normalized, b.directions.ToVector()) > 0.5;
            bool distanceCheck = Vector3.Distance(b.position, a.position) < connectLooseDistance ;
            bool parentCheck = a.generator != b.generator;
            return angleCheck && distanceCheck && parentCheck;
        }

        Floor GenerateFloor (Rect options)
        {
            GameObject roomObj = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			roomObj.transform.SetParent(generatorParent.transform);
            Floor room = roomObj.GetComponent<Floor>();

            room.size = options;


			tileDrawer.startCollisionTest ();
			room.tilePool = tilePool;
			room.tileDrawer = tileDrawer;
			room.Build();

			if (tileDrawer.getCollisionTestResult()) {
				Destroy(roomObj);
				return null;
			}


            foreach (var g in generators)
            {
				var roomConfigurable = (IForFloor)g;
				if (roomConfigurable != null) {
					roomConfigurable.floor = room;
				}
				if (g.tilePool == null) {
					g.tilePool = this.tilePool;
				}
                g.Generate(seed: Random.Range(0, 65536));
                g.Draw(tileDrawer: tileDrawer);
            }

           
            return room;
        }

        void AddWalls() {
            foreach (var floor in floors) {
                var shouldPlaceWalls = Random.value <= wallProbability.Evaluate(floor.BoundsGrid.center.magnitude);

                if (!shouldPlaceWalls) {
                    continue;
                }

				var walls = floor.gameObject.AddComponent<WallsForRoom>();
				walls.height = 1f;
				walls.room = floor;
				walls.openingsForDoors = true;
				walls.seed = Random.Range(0, 65536);
				walls.tilePool = tilePool;
				walls.tileDrawer = tileDrawer;
				walls.Generate(seed: walls.seed);
				walls.Draw(tileDrawer:tileDrawer, tilePool:tilePool);
			}
		}
        void AddDepth(){
            foreach (var room in floors) {
                depthGenerator.room = room;
                depthGenerator.Generate (seed: Random.Range (0, 65536));
                depthGenerator.Draw (tileDrawer: tileDrawer);
            }
        } 

        Hallway GenerateHallway(Vector3 startPoint, Vector3 endPoint, bool ignoreCollision=false)
        {
            GameObject hallwayObj = Instantiate(hallwayPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			hallwayObj.transform.SetParent(generatorParent.transform);
            Hallway hallway = hallwayObj.GetComponent<Hallway>();

			hallway.tilePool = tilePool;
			hallway.tileDrawer = tileDrawer;
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