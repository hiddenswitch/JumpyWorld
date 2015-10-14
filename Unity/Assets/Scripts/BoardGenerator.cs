using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
namespace JumpyWorld
{
    public class BoardGenerator : MonoBehaviour
    {
		public GameObject[] groundBox;
		public int columns=30;
		public int rows=30;
		public int columnsPerLevel=3;
		public int level=1;
		public GameObject[] dangers;
		public GameObject[] walls;

		private Transform boardHolder;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void setupScene()
        {
			BoardSetup ();
        }
		void BoardSetup()
		{
			int height;
			boardHolder= new GameObject("Board").transform;
			for (int x=-1; x<columns;x++)
			{
				for (int z=-1; z<rows;z++)
				{
					GameObject[] objectType= groundBox;
					level= (int)(Math.Max(x,z)/columnsPerLevel);

					if(Random.Range (0,50)<2*level){
						height=1;
					}
					else if(Random.Range (0,50)<level){
						height=-1;						
					}
					else{
						if(Random.Range (0,50)<level){
							objectType=dangers;
						}
						height=0;
					}
					DrawTerrain(height,objectType,new Vector3(x,height,z));
				
				}
			}
		}
		void LayoutObject(int Density, GameObject[] objects,int x, int row){

		}
		void addDangers(){

		}
		void DrawTerrain(int height, GameObject[] type, Vector3 loc){

			if (height!=-1){
				for (int y =-1; y< height-1; y++) {
					GameObject ground=Instantiate (groundBox[0], loc, Quaternion.identity) as GameObject;
					ground.transform.SetParent (boardHolder);
				}
				GameObject toInstantiate = type [Random.Range (0, type.Length-1)];
				GameObject instance = Instantiate (toInstantiate, loc, Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
    }
}