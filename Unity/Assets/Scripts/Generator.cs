using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class Generator : Placeable
	{
		public TileDrawer tileDrawer;
		public TilePool tilePool;
		public int seed;
		public Anchor[] anchors;
        public bool generateOnStart = false;

        void Start ()
        {
            if (generateOnStart)
            {
                Build ();
            }
        }

		public void Build ()
		{
			tileDrawer = tileDrawer ?? GetComponent<TileDrawer> () ?? TileDrawer.instance;
			tilePool = tilePool ?? GetComponent<TilePool> () ?? TilePool.instance;

			var oldSeed = Random.seed;
			Random.seed = seed;

			Generate (seed: seed);
			Draw (tileDrawer: tileDrawer, tilePool: tilePool);

			Random.seed = oldSeed;
		}

		public virtual void Generate (int seed)
		{

		}

		public virtual void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{

		}

		
		public virtual void OnDrawGizmos ()
		{
			Gizmos.color = Color.blue;
			foreach (var anchor in anchors) {
				Gizmos.DrawWireCube (anchor.position, new Vector3 (1, 1, 1));
			}
		}
	}
}
