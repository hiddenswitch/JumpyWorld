using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class TerrainDrawer : MonoBehaviour
	{
		public Transform parent;
		public LayerMask terrainLayer;
		/// <summary>
		/// This bloom filter stores whether or not tiles exist at a certain position. A very space efficient data structure.
		/// </summary>
		private BloomFilter.Filter<Vector3> bloomFilter;

		TerrainDrawer ()
		{
			bloomFilter = new BloomFilter.Filter<Vector3> (capacity: 100000, hashFunction: HashFunction);
		}

		int HashFunction (Vector3 value)
		{
			return value.GetHashCode ();
		}

		// Use this for initialization
		void Start ()
		{
			parent = parent ?? transform;
		}

		public void DrawTerrain (GameObject tile=null, Vector3 at=default(Vector3))
		{
			// Don't place terrain if it already exists
			at = Tile.ToGrid (at);

			if (bloomFilter.Contains (at)) {
				// Bloom filters are only probabilistically accurate for positives.
				// Check for certain if a tile exists there
				var colliders = Physics.OverlapSphere (at, Tile.gridSize / 2f);
				foreach (var collider in colliders) {
					// Is this a terrain tile?
					if (((1 << collider.gameObject.layer) & terrainLayer.value) != 0) {
						// A tile was found. Do not create a tile
						// TODO: Replace it?
						return;
					}
				}
			}

			GameObject instance = Instantiate (tile, at, Quaternion.identity) as GameObject;
			bloomFilter.Add (at);
			instance.transform.SetParent (parent, worldPositionStays: false);
		}
	}
}