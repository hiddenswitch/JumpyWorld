using UnityEngine;
using System.Collections;

namespace JumpyWord
{
	public class BelowDepth : MonoBehaviour
	{

		public float depth;
		public GameObject recipient;
		public string message = "Fell";
		public float debounce = 0.3f;
		float debounceTime = 0f;
		
		// Use this for initialization
		void Start ()
		{
		}
		
		// Update is called once per frame
		void Update ()
		{
			if (transform.position.y < depth
				&& debounceTime + debounce < Time.time) {
				debounceTime = Time.time;
				recipient.SendMessage (message, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}