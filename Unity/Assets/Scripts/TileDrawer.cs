using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class TileDrawer : MonoBehaviour, IList<Vector3>
	{
		struct TileInfo
		{
			public Vector3 position;
			public GameObject gameObject;

			public override int GetHashCode ()
			{
				return position.GetHashCode () ^ gameObject.GetHashCode () << 1;
			}
		}

		public static TileDrawer instance { get; private set; }

		public Transform parent;
		public LayerMask terrainLayer;
		private Dictionary<Vector3, TileInfo> tiles = new Dictionary<Vector3, TileInfo> ();
		private bool hasCollided = false;

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

			if (this.Contains (at)) {
				return;
			}

			GameObject instance = Instantiate (tile, at, Quaternion.identity) as GameObject;
			tiles.Add (at, new TileInfo () {position = at, gameObject = instance});
			instance.transform.SetParent (parent, worldPositionStays: false);
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
			foreach (var kv in tiles) {
				Destroy (kv.Value.gameObject);
			}

			tiles.Clear ();
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