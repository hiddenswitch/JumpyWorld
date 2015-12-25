using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class FXMaker : MonoBehaviour
	{
		public GameObject effect;
		public Vector3 offset;
		public Vector3 scale;
		public Quaternion rotation;

		public void FX() {
			var instance = CFX_SpawnSystem.GetNextObject (effect);
			instance.transform.position = transform.position + offset;
			instance.transform.localScale = scale;
			instance.transform.rotation = rotation;
		}
	}
}