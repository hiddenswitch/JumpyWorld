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

		void Start ()
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
	}
}
