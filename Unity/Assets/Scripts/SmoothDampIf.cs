using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class SmoothDampIf : MonoBehaviour
	{
		[Header ("Required")]
		public SmoothDampXZCamera smoothDamp;
		[Header ("Options")]
		public float minY = -1f;
	
		// Update is called once per frame
		void Update ()
		{
			if (smoothDamp.target.position.y < minY) {
				smoothDamp.enabled = false;
			} else {
				smoothDamp.enabled = true;
			}
		}
	}
}