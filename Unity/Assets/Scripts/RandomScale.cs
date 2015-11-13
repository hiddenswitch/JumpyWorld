using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RandomScale : MonoBehaviour
	{
		public AnimationCurve scaleDensity;
		// Use this for initialization
		void Start ()
		{
			var scale = scaleDensity.Evaluate (Random.value);
			transform.localScale = new Vector3 (scale, scale, scale);
		}
	}

}