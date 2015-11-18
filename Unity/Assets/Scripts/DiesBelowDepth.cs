using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class DiesBelowDepth : MonoBehaviour
	{
		public float depth;
		public GameObject[] eventHandlers;
		public string message = "OnObjectDied";
		[Header("Runtime")]
		public bool
			isAlive = true;
		// Use this for initialization
	
		// Update is called once per frame
		void Update ()
		{
			if (transform.position.y < depth
				&& isAlive) {
				isAlive = false;
				foreach (var handler in eventHandlers) {
					handler.SendMessage (message, this.gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		public void ResetDies ()
		{
			isAlive = true;
		}
	}
}
