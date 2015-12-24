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

		public void OnObjectRevived (GameObject sender)
		{
			StartCoroutine (OnObjectRevivedDelayed (sender));
		}

		IEnumerator OnObjectRevivedDelayed (GameObject sender)
		{
			yield return new WaitForSeconds (delay);
			RandomizeAndRebuild ();
		}
	}
}