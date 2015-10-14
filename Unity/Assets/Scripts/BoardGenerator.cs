using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
namespace JumpyWorld
{
    public class BoardGenerator : MonoBehaviour
    {
		public GameObject groundBox;
		public int columns;
		public int rows;
		public int maxHeight=3;
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
			boardHolder= new GameObject("Board").transform;
			for (int x=-1; x<columns;x++)
			{
				for (int z=-1; z<rows;z++)
				{
					for (int y =-1 ; y<Random.Range(-1,maxHeight);y++){
						print(Random.Range(-1,5));
						GameObject instance= Instantiate (groundBox, new Vector3(x,y,z),Quaternion.identity)as GameObject;
						instance.transform.SetParent(boardHolder);
					}
				}
			}
		}
    }
}