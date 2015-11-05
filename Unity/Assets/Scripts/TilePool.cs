using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class TilePool : MonoBehaviour
	{
		public static TilePool instance { get; private set; }

		public GameObject defaultGround;

		void Awake ()
		{
			instance = this;
		}
	}
}
