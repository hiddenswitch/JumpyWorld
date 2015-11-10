using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class TileDrawer : MonoBehaviour
	{
		public static TileDrawer instance { get; private set; }

		public Transform parent;
		public LayerMask terrainLayer;
		/// <summary>
		/// This bloom filter stores whether or not tiles exist at a certain position. A very space efficient data structure.
		/// </summary>
		private BloomFilter.Filter<Vector3> bloomFilter;

		private bool hasCollided = false;

		TileDrawer ()
		{
			bloomFilter = new BloomFilter.Filter<Vector3> (capacity: 100000, hashFunction: HashFunction);
		}

		void Awake ()
		{
			instance = this;
			parent = parent ?? transform;
		}

		int HashFunction (Vector3 value)
		{
			return value.GetHashCode ();
		}

		public void DrawTerrain (GameObject tile=null, Vector3 at=default(Vector3))
		{
			// Don't place terrain if it already exists
			at = Tile.ToGrid (at);

			if (bloomFilter.Contains (at)) {
				// Bloom filters are only probabilistically accurate for positives.
				// Check for certain if a tile exists there
				var colliders = Physics.OverlapSphere (at, Tile.gridSize / 2f - Physics.defaultContactOffset);
				foreach (var collider in colliders) {
					// Is this a terrain tile?
					if (((1 << collider.gameObject.layer) & terrainLayer.value) != 0) {
						// A tile was found. Do not create a tile
						// TODO: Replace it?
						hasCollided = true;
						Debug.Log ("collision at "+ at);
						return;
					}
				}
			}

			GameObject instance = Instantiate (tile, at, Quaternion.identity) as GameObject;
			bloomFilter.Add (at);
			instance.transform.SetParent (parent, worldPositionStays: false);
		}

		public bool TestCollision (Vector3 at=default(Vector3)){
			// Don't place terrain if it already exists
			at = Tile.ToGrid (at);
			
			if (bloomFilter.Contains (at)) {
				// Bloom filters are only probabilistically accurate for positives.
				// Check for certain if a tile exists there
				var colliders = Physics.OverlapSphere (at, Tile.gridSize / 2f - Physics.defaultContactOffset);
				foreach (var collider in colliders) {
					// Is this a terrain tile?
					if (((1 << collider.gameObject.layer) & terrainLayer.value) != 0) {
						// A tile was found. Do not create a tile
						return true;
					}
				}
			}
			return false;
		}

		public void startCollisionTest(){
			hasCollided = false;
		}

		public bool getCollisionTestResult(bool reset = true){
			bool result = hasCollided;
			if (reset) {
				hasCollided = false;
			}
			return result;
		}
	}
}