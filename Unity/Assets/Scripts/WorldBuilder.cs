using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class WorldBuilder : MonoBehaviour
	{
		public TileDrawer tileDrawer;
		public TilePool tilePool;
		// Use this for initialization
		void Start ()
		{
			tileDrawer = tileDrawer ?? GetComponent<TileDrawer> () ?? TileDrawer.instance;
			tilePool = tilePool ?? GetComponent<TilePool> () ?? TilePool.instance;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}
}