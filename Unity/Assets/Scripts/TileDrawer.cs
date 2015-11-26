using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class TileDrawer : MonoBehaviour, IList<Vector3>
	{
		class TileInfo
		{
			public Vector3 position;
			public GameObject gameObject;
			public GameObject prefab;
			public bool isDynamic;

			public override int GetHashCode ()
			{
				return position.GetHashCode ();
			}

        }

		public static TileDrawer instance { get; private set; }

		public int batchSize;
		public Transform parent;
		public LayerMask terrainLayer;
		private GameObject destroyableParent;
		
		public bool isDrawingTiles = false;

		private Dictionary<Vector3, TileInfo> tiles = new Dictionary<Vector3, TileInfo> (400);
		private Queue<TileInfo> batchQueue = new Queue<TileInfo> ();
		private bool hasCollided = false;
		private bool clearing = false;

		void Awake ()
		{
			instance = this;
			parent = parent ?? transform;
			CreateDestroyableParent ();
		}

		void Start ()
		{
			StartCoroutine (QueueProcessingCoroutine ());
		}

		int HashFunction (Vector3 value)
		{
			return value.GetHashCode ();
		}

		void CreateDestroyableParent ()
		{
			destroyableParent = new GameObject ("Destroyable Parent");
			destroyableParent.transform.SetParent (parent);
		}

		public void DrawTerrain (GameObject prefab=null, Vector3 at=default(Vector3), bool isDynamic=true, bool overwriteIfExists = false)
		{
			// Don't place terrain if it already exists
			at = Tile.ToGrid (at);

			if (this.Contains (at)) {
                if (!overwriteIfExists)
                {
                    return;
                } else
                {
                    var infoToBeReplaced = tiles[at];
                    if (batchQueue.Contains(infoToBeReplaced))
                    {
                        infoToBeReplaced.prefab = prefab;
                        infoToBeReplaced.isDynamic = isDynamic;
                        return;
                    } else
                    {
                        Destroy(infoToBeReplaced.gameObject);
                        tiles.Remove(at);
                    }
                }
            }

			var tileInfo = new TileInfo () {position = at, prefab = prefab, isDynamic = isDynamic};

			batchQueue.Enqueue (tileInfo);
			tiles.Add (at, tileInfo);
		}
		
		IEnumerator QueueProcessingCoroutine ()
		{
			var tilesInBatch = new List<TileInfo> (batchSize);
			var batched = 0;
			while (true) {
				while (batchQueue.Count > 0) {
					isDrawingTiles = true;
					var tileInfo = batchQueue.Dequeue ();
					tileInfo.gameObject = InstantiateTile (tileInfo);
					tilesInBatch.Add (tileInfo);
					batched++;
					if (batched > batchSize) {
						var batchParent = new GameObject ("Batch Parent");
						batchParent.transform.SetParent (destroyableParent.transform);
						foreach (var batchTileInfo in tilesInBatch) {
							if (!batchTileInfo.isDynamic) {
								var tile = batchTileInfo.gameObject;
								tile.transform.SetParent (batchParent.transform, true);
							}
						}

						StaticBatchingUtility.Combine (batchParent);

						yield return new WaitForEndOfFrame ();

						if (clearing) {
							Destroy (batchParent);
						}
						
						batched = 0;
						tilesInBatch.Clear ();
					}
				}
				if (batchQueue.Count == 0){
					isDrawingTiles = false;
				}
				if (tilesInBatch.Count > 0
					&& !clearing) {
					var batchParentFinal = new GameObject ("Batch Parent");
					batchParentFinal.transform.SetParent (destroyableParent.transform);
					StaticBatchingUtility.Combine (batchParentFinal);
				}
				batched = 0;
				tilesInBatch.Clear ();
				yield return null;
			}
		}

		GameObject InstantiateTile (TileInfo tileInfo)
		{
			var tile = tileInfo.prefab;
			var at = tileInfo.position;
			var instance = tileInfo.gameObject = Instantiate (tile, at, Quaternion.identity) as GameObject;
			instance.transform.SetParent (destroyableParent.transform, worldPositionStays: false);
			return instance;
		}

		public bool TestCollision (Vector3 at=default(Vector3))
		{
			return this.Contains (at);
		}

		public void startCollisionTest ()
		{
			hasCollided = false;
		}

		public bool getCollisionTestResult (bool reset = true)
		{
			bool result = hasCollided;
			if (reset) {
				hasCollided = false;
			}
			return result;
		}

		#region IList implementation

		public int IndexOf (Vector3 item)
		{
			throw new System.NotImplementedException ();
		}

		public void Insert (int index, Vector3 item)
		{
			throw new System.NotImplementedException ();
		}

		public void RemoveAt (int index)
		{
			throw new System.NotImplementedException ();
		}

		public Vector3 this [int index] {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		#endregion

		#region ICollection implementation

		public void Add (Vector3 item)
		{
			throw new System.NotImplementedException ();
		}

		public void Clear ()
		{
			clearing = true;
			tiles.Clear ();
			batchQueue.Clear ();
			Destroy (destroyableParent);
			CreateDestroyableParent ();
			// TODO: Investigate the correctness of doing this
			clearing = false;
		}

		public bool Contains (Vector3 item)
		{
			return tiles.ContainsKey (item);
		}

		public void CopyTo (Vector3[] array, int arrayIndex)
		{
			throw new System.NotImplementedException ();
		}

		public bool Remove (Vector3 item)
		{
			if (!tiles.ContainsKey (item)) {
				return false;
			}

			Destroy (tiles [item].gameObject);
			return tiles.Remove (item);
		}

		public int Count {
			get {
				return tiles.Count;
			}
		}

		public bool IsReadOnly {
			get {
				throw new System.NotImplementedException ();
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<Vector3> GetEnumerator ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion
	}
}