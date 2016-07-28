using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class SetFramerate : MonoBehaviour
	{
		// Use this for initialization
		void Awake ()
		{
			Application.targetFrameRate = -1;
		}
	}
}