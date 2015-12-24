using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class Killable : PinBool
	{
		public GameObject[] eventHandlers;
		public string message = "OnObjectDied";
		public string reviveMessage = "OnObjectRevived";
		[Header ("Runtime")]
		public bool
			isAlive = true;

		public override bool value {
			get {
				return isAlive;
			}
			set {
				isAlive = value;
			}
		}

		public void Die ()
		{
			if (isAlive) { 
				isAlive = false;
				foreach (var handler in eventHandlers) {
					handler.SendMessage (message, this.gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		public void Revive ()
		{
			ResetDies ();
		}

		public void ResetDies ()
		{
			if (!isAlive) {
				isAlive = true;
				foreach (var handler in eventHandlers) {
					handler.SendMessage (reviveMessage, this.gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
