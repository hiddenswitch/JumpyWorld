using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RandomOrientation : MonoBehaviour
	{
		public int axis = 1;
		// Use this for initialization
		void Start ()
		{
			var rotation = transform.rotation.eulerAngles;
			rotation [axis] = Random.value * 360f;
			transform.rotation = Quaternion.Euler (rotation);
		}
	}
}
