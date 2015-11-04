using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace JumpyWorld
{
	public class BoardGenerator : MonoBehaviour
	{
		public enum BoardStyle
		{
			Path,
			Room
		}
		public bool generateOnStart;
		public BoardStyle style;
		public int seed = 101;
        /// <summary>
        /// This is not a vary functional int yet, I will come up with a functional way to make the paths less or more blobby, I think right now it works to make things more bloby(larger number is more fully covered room area)The elongation factor.
        /// </summary>
        public int elongationFactor=2;
		public GameObject[] groundBox;
		public int columns = 30;
		public int rows = 30;
		public int columnsPerLevel = 2;
		public GameObject[] dangers;
        public GameObject[] passableDangers;// things that can be deactivated or walked through, such as spikes from Quest Keeper
		public GameObject[] walls;
		public Transform boardParent;     
		public Vector3 endpoint = new Vector3 (5, -1, 5);
		public Vector3 startpoint = new Vector3 (0, -1, 0);
		private Transform boardHolder;
        private int level = 1;
    
//		private List<Vector3> gridPositions = new List<Vector3> ();
		private List<Vector3> pathPositions = new List<Vector3> ();
        private List<Vector3> pathBackBone = new List<Vector3> ();

		void Start ()
		{
			var oldSeed = UnityEngine.Random.seed;
			UnityEngine.Random.seed = seed;
			switch (style) {
			case BoardStyle.Path:
				CreatePath (elongationFactor);
				break;
			case BoardStyle.Room:
				CreateRoom ();
				break;
			}
			UnityEngine.Random.seed = oldSeed;
		}

		void LayoutObject (int density, GameObject[] objects, int x, int row)
		{
			// TODO: Places objects along a given position with a given density
		}

		void AddDangers ()
		{
			// TODO: Add dangers to the map
		}

		void CreatePath (int elongation)
		{
			// TODO: Create an exciting path between two points
            //Bresenham's algorithm

           pathBackBone=(StraighPath(startpoint,endpoint,false,5));
            //pathPositions=(StraighPath(new Vector3(0,0,0), new Vector3(5,0,13), true, 1));
            for (int i=0; i < 10; i++) {
               PertrubePath(pathBackBone);
            }
            for (int i =1; i < pathBackBone.Count; i++) {
                pathPositions.AddRange(StraighPath(pathBackBone[i-1],pathBackBone[i],true,1));
            }

            for (int i =0; i< pathPositions.Count; i++) {
                DrawTerrain(0,groundBox,pathPositions[i]);
            }
		}
        List<Vector3> StraighPath(Vector3 startpt, Vector3 endpt, Boolean pathable, int density){
            List<Vector3>path= new List<Vector3>();
            Vector3 start=startpt;
            Vector3 end=endpt;
            Vector3 directionLine= endpt - startpt;
            float m = (float)directionLine [2] / (float)directionLine [0];

            float epsilon = m - 1.0f;
            print ("what"+epsilon);

            int drivingAxis=0;
            int otherAxis = 2;
            if (Math.Abs(directionLine [2]) > Math.Abs (directionLine [0])){
                drivingAxis=2;
                otherAxis=0;
                print ("changgeee");
                print (epsilon);
                epsilon=(1.0f/m)-1;
                m=1.0f/m;
                print (epsilon);
            }
            if (startpt [drivingAxis] > endpt [drivingAxis]) {
                start= endpt;
                end= startpt;
            }
            Vector3 currentPoint = start;
            Vector3 connectingPoint = new Vector3 (0, 0, 0);
            connectingPoint[drivingAxis]=1;

            for (int i = (int)start[drivingAxis]; i<=end[drivingAxis];i=i+1){
                currentPoint[drivingAxis]=i;
                if (i%density==0){
                path.Add(currentPoint);
                if (pathable && !path.Contains(currentPoint+connectingPoint)){
                        path.Add (currentPoint+connectingPoint);}}
                if (epsilon>1.0){
                    currentPoint[otherAxis]+=1;
                    epsilon-=1.0f;
                }
                epsilon+=m;
            }
            if (!path.Contains(end)){
                path.Add(end);
            }

            return path;
        }
		void CreateRoom ()
		{
			int height;
			boardHolder = new GameObject ("Board").transform;
            CreatePath (0);
			for (int x=-1; x<columns; x++) {
				for (int z=-1; z<rows; z++) {
					GameObject[] objectType = groundBox;
					level = (int)(Math.Max (x, z) / columnsPerLevel);
					if (!pathPositions.Contains (new Vector3 (x, -1, z))) {
						if (Random.Range (0, 50) < 1 * level) { //walls
							height = 1;
						} else if (Random.Range (0, 50) < level) {//holes
							height = -1;						
						} else {
							if (Random.Range (0, 50) < level) {//dangers
								objectType = dangers;
							}
							height = 0;//normal ground otherwise
						}
						DrawTerrain (height, objectType, new Vector3 (x, -1, z));
						
					}
				}
			}

			if (boardParent != null) {
				boardHolder.SetParent (boardParent, false);
			}
		}
        void PertrubePath(List<Vector3> path){
            var dir = new List<Vector3>{new Vector3(0,0,2), new Vector3(0,0,-2),new Vector3(2,0,0),new Vector3(-2,0,0)};
            int index =  Random.Range (1, (path.Count-1));
            Vector3 movePoint = path [index];
            path [index] = movePoint + dir [Random.Range (0, dir.Count)];
        }
		Vector3 PickNeighbor (Vector3 pt,int e)
		{
//            int index;
//            int indexOfLast;
//            var dir = new List<Vector3>{new Vector3(0,0,1), new Vector3(0,0,-1),new Vector3(1,0,0),new Vector3(-1,0,0)};
//			if (pt [2] >= rows - 1) {
//				dir.Remove (new Vector3(0,0,1));
//			} else if (pt [2] <= 0) {
//				dir.Remove (new Vector3(0,0,-1));
//			}
//			if (pt [0] >= columns - 1) {
//                dir.Remove (new Vector3 (1, 0, 0));
//            } else if (pt [0] <= 0) {
//                dir.Remove(new Vector3(-1,0,0));             
//               }
//            if (dir.Contains (lastDirection)) {
//                indexOfLast = dir.IndexOf (lastDirection);
//                index = Random.Range (0, dir.Count + e);
//                if (index >= dir.Count) {
//                    index = indexOfLast;
//                }
//            } else {
//                index = Random.Range (0, dir.Count);
//            }
//			Vector3 newpt = pt;
//            print (dir.Count);
//            print (index);
//			while (visitedPositions.Contains(newpt)&& dir.Count>0) {
//                newpt = pt + dir [index];
//                lastDirection = dir [index];
//                dir.RemoveAt (index);
//                index = Random.Range (0, dir.Count);   
//                print ("this is the index: "+ index);
//                print ("this is the dir"+ dir.Count);
//               				
//			}
//			return newpt;
            return new Vector3 ();
		}

		void DrawTerrain (int height, GameObject[] type, Vector3 loc)
		{

			if (height != -1) {
				for (int y =0; y< height; y++) {
					GameObject ground = Instantiate (groundBox [0], loc + new Vector3 (0, y, 0), Quaternion.identity) as GameObject;
					ground.transform.SetParent (boardHolder);
				}
				GameObject toInstantiate = type [Random.Range (0, type.Length - 1)];
				GameObject instance = Instantiate (toInstantiate, loc + new Vector3 (0, height, 0), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}
}