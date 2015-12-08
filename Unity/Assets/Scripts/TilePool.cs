using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class TilePool : MonoBehaviour
	{
		public static TilePool instance { get; private set; }

		public GameObject defaultGround;
		/// <summary>
		/// An empty or special gameobject to use to represent a path
		/// </summary>
		public GameObject path;
		public GameObject defaultEmpty;
		public GameObject monsterPlacer;
		public GameObject[] decorative;

		void Awake ()
		{
			instance = this;
		}
	}
}
