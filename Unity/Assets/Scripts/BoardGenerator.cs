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
		public GameObject[] groundBox;
		public int columns = 30;
		public int rows = 30;
		public int columnsPerLevel = 3;
		public int level = 1;
		public GameObject[] dangers;
		public GameObject[] walls;
		public Transform boardParent;
		public Vector3 endpoint = new Vector3 (29, -1, 29);
		public Vector3 startpoint = new Vector3 (0, -1, 0);
		private Transform boardHolder;
		private List<Vector3> gridPositions = new List<Vector3> ();
		private List<Vector3> pathPositions = new List<Vector3> ();
		// Use this for initialization
		void Start ()
		{
			if (generateOnStart) {
				setupScene ();
			}
		}

		// Update is called once per frame
		void Update ()
		{

		}

		public void setupScene ()
		{
			BoardSetup ();
		}

		void BoardSetup ()
		{
			switch (style) {
			case BoardStyle.Path:
				createPath ();
				break;
			case BoardStyle.Room:
				createRoom ();
				break;
			}

		}

		void LayoutObject (int Density, GameObject[] objects, int x, int row)
		{

		}

		void addDangers ()
		{

		}

		void createPath ()
		{

			Vector3 currentPoint = startpoint;
			//while(currentPoint!=endpoint){
			for (int i=0; i <rows+columns; i++) {
				DrawTerrain (0, groundBox, currentPoint);
				pathPositions.Add (currentPoint);
				currentPoint = pickNeighbor (currentPoint);
			}

		}

		void createRoom ()
		{
			int height;
			boardHolder = new GameObject ("Board").transform;

			for (int x=-1; x<columns; x++) {
				for (int z=-1; z<rows; z++) {
					GameObject[] objectType = groundBox;
					level = (int)(Math.Max (x, z) / columnsPerLevel);
					if (!pathPositions.Contains (new Vector3 (x, -1, z))) {
						if (Random.Range (0, 50) < 1 * level) {
							height = 1;
						} else if (Random.Range (0, 50) < level) {
							height = -1;						
						} else {
							if (Random.Range (0, 50) < level) {
								objectType = dangers;
							}
							height = 1;
						}
						DrawTerrain (height, objectType, new Vector3 (x, -1, z));
						
					}
				}
			}

			if (boardParent != null) {
				boardHolder.SetParent(boardParent, false);
			}
		}

		Vector3 pickNeighbor (Vector3 pt)
		{
			var dir = new List<Vector3>{new Vector3(0,0,1), new Vector3(0,0,-1), new Vector3(-1,0,0),new Vector3(1,0,0)};
			if (pt [2] == rows - 1) {
				dir.RemoveAt (0);
			} else if (pt [2] == 0) {
				dir.RemoveAt (1);
			}
			if (pt [0] == columns - 1) {
				dir.Remove (new Vector3 (1, 0, 0));
			} else if (pt [0] == 0) {
				dir.Remove (new Vector3 (-1, 0, 0));
			}
			int index = Random.Range (0, dir.Count - 1);
			Vector3 newpt = pt;
			while (pathPositions.Contains(newpt)&& dir.Count>1) {
				newpt = pt + dir [index];
				dir.RemoveAt (index);
				index = Random.Range (0, dir.Count - 1);
				newpt = pt + dir [index];

			}
			return newpt;
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