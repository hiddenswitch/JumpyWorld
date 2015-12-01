using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ClearAndRebuildWorld : MonoBehaviour
	{
		public WorldBuilder worldBuilder;
		public TileDrawer tileDrawer;
		public float delay = 2f;

		void Awake ()
		{
			RandomizeSeed ();
		}

		public void RandomizeSeed ()
		{
			worldBuilder.seed = Random.Range (1, 65536);
		}

		public void RandomizeAndRebuild ()
		{
			RandomizeSeed ();
			tileDrawer.Clear ();
			worldBuilder.Start ();
		}

		public void OnObjectDied (GameObject sender)
		{
			StartCoroutine (OnObjectDiedDelayed (sender));
		}

		IEnumerator OnObjectDiedDelayed (GameObject sender)
		{
			yield return new WaitForSeconds (delay);
			RandomizeAndRebuild ();
		}
	}
}