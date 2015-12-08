using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class TilePool : MonoBehaviour
	{
		public static TilePool instance { get; private set; }

		public GameObject defaultGround;
		public GameObject defaultEmpty;
		public GameObject monsterPlacer;
		public GameObject[] decorative;

		void Awake ()
		{
			instance = this;
		}
	}
}
