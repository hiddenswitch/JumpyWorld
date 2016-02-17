using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ConditionalKillOnTouch : MonoBehaviour
	{
		public ICondition condition;
		public string killMessage;
		public GameObject[] eventHandlers;
		public LayerMask triggersWith;

		// Use this for initialization
		void Start ()
		{
			this.condition = condition ?? GetComponent<ICondition> ();
		}

		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerStay (Collider other)
		{
			var conditionMet = condition == null ? true : condition.evaluate ();
			if (conditionMet && ((1 << other.gameObject.layer) & triggersWith.value) > 0) {
				other.gameObject.SendMessageUpwards ("Die");
			}
		}
	}
}