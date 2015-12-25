using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class FXOnDeath : FXMaker
	{
		void OnObjectDied (GameObject sender)
		{
			FX ();
		}
	}
}