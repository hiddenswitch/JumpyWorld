using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class TileDrawer : PinBool, IList<Vector3>
	{
		class TilesRecord
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

		static Vector3 one = new Vector3 (1f, 1f, 1f);

		public static TileDrawer instance { get; private set; }

		public int batchSize;
		public Transform parent;
		public LayerMask terrainLayer;
		public GameObject destroyableParent;
		
		public bool isDrawingTiles = false;

		public override bool value {
			get {
				return isDrawingTiles;
			}
			set {
				isDrawingTiles = value;
			}
		}

		private Dictionary<Vector3, TilesRecord> tiles = new Dictionary<Vector3, TilesRecord> (400);
		private Queue<TilesRecord> batchQueue = new Queue<TilesRecord> ();
		private HashSet<TilesRecord> batchQueueSet = new HashSet<TilesRecord> ();
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

		public void DrawTerrain (GameObject prefab = null, Vector3 at = default(Vector3), bool isDynamic = true, bool overwriteIfExists = false)
		{
			
			at = Tile.ToGrid (at);
			// Get information about what we're drawing
			var tile = prefab.GetComponent<Tile> ();
			var volume = one;
			var pivot = Vector3.zero;
			if (tile != null) {
				volume = tile.size;
				pivot = tile.pivot;
			}

			// Don't place terrain if it already exists
			var queueOverwritten = false;
			foreach (var unadjustedPoint in TileDrawer.Volume(volume)) {
				var point = unadjustedPoint + at - pivot;
				if (this.Contains (point)) {
					if (!overwriteIfExists) {
						return;
					} else {
						var recordAtPoint = tiles [point];

						if (batchQueueSet.Contains (recordAtPoint)) {
							if (unadjustedPoint == Vector3.zero) {
								recordAtPoint.prefab = prefab;
								recordAtPoint.isDynamic = isDynamic;
							} else {
								batchQueueSet.Remove (recordAtPoint);
							}
							queueOverwritten = true;
						} else {
							Destroy (recordAtPoint.gameObject);
							tiles.Remove (point);
						}
					}
				}
			}

			if (queueOverwritten) {
				return;
			}

			var tileRecord = new TilesRecord () { position = at, prefab = prefab, isDynamic = isDynamic };

			batchQueue.Enqueue (tileRecord);
			batchQueueSet.Add (tileRecord);

			foreach (var unadjustedPoint in TileDrawer.Volume(volume)) {
				var point = at + unadjustedPoint - pivot;
				tiles.Add (point, tileRecord);
			}
		}

		public static IEnumerable<Vector3> Volume (Vector3 volume)
		{
			for (var x = 0f; x < volume.x; x++) {
				for (var y = 0f; y < volume.y; y++) {
					for (var z = 0f; z < volume.z; z++) {
						yield return new Vector3 (x, y, z);
					}
				}
			}
		}

		IEnumerator QueueProcessingCoroutine ()
		{
			var tilesInBatch = new List<TilesRecord> (batchSize);
			var batched = 0;
			while (true) {
				while (batchQueue.Count > 0) {
					isDrawingTiles = true;
					var tileRecord = batchQueue.Dequeue ();
					if (!batchQueueSet.Contains (tileRecord)) {
						continue;
					}
					tileRecord.gameObject = InstantiateTile (tileRecord);
					tilesInBatch.Add (tileRecord);
					batchQueueSet.Remove (tileRecord);
					batched++;
					if (batched > batchSize) {
						var batchParent = new GameObject ("Batch Parent");
						batchParent.transform.SetParent (destroyableParent.transform);
						foreach (var batchTileRecord in tilesInBatch) {
							if (!batchTileRecord.isDynamic) {
								var tile = batchTileRecord.gameObject;
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
				if (batchQueue.Count == 0) {
					isDrawingTiles = false;
				}
				if (tilesInBatch.Count > 0
				    && !clearing) {
					var batchParentFinal = new GameObject ("Batch Parent");
					batchParentFinal.transform.SetParent (destroyableParent.transform);
					StaticBatchingUtility.Combine (batchParentFinal);
				}
				Debug.Log (string.Format ("batched {0}", batched));
				batched = 0;
				tilesInBatch.Clear ();
				yield return null;
			}
		}

		GameObject InstantiateTile (TilesRecord tileRecord)
		{
			var tile = tileRecord.prefab;
			var at = tileRecord.position;
			var instance = tileRecord.gameObject = Instantiate (tile, at, Quaternion.identity) as GameObject;
			instance.transform.SetParent (destroyableParent.transform, worldPositionStays: false);
			return instance;
		}

		public bool TestCollision (Vector3 at = default(Vector3))
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