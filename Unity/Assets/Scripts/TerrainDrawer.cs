using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class TerrainDrawer : MonoBehaviour
	{
		public Transform parent;
		// Use this for initialization
		void Start ()
		{
			parent = parent ?? transform;
		}

		public void DrawTerrain (GameObject tile=null, Vector3 at=default(Vector3))
		{
			GameObject instance = Instantiate (tile, at, Quaternion.identity) as GameObject;
			instance.transform.SetParent (parent, worldPositionStays: false);
		}
	}
}