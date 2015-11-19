using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ThemeChanger : MonoBehaviour
	{
		public Theme[] themes;
		public WorldBuilder worldBuilder;
		public GameObject cameraBackground;

		public void RandomizeTheme ()
		{
			var theme = themes [Random.Range (0, themes.Length)];
			// TODO: This is just supporting the handful of things we actually want to do.
			worldBuilder.tilePool = theme.tilePool;
			cameraBackground.GetComponent<MeshRenderer> ().material = theme.cameraBackground;
		}
	}
}