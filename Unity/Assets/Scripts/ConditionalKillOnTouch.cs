using UnityEngine;
using System.Collections;

namespace JumpyWorld {
    [RequireComponent(typeof(ICondition))]
    public class ConditionalKillOnTouch : MonoBehaviour {
        public ICondition condition;
        public string killMessage;
        public GameObject[] eventHandlers;
        public LayerMask triggersWith;

        // Use this for initialization
        void Start () {
            this.condition = condition ?? GetComponent<ICondition>();
        }

        // Update is called once per frame
        void Update () {
	
	    }
        void OnTriggerEnter(Collider other)
        {
            if (condition.evaluate() && ((1 << other.gameObject.layer) & triggersWith.value) > 0)
            {
                Debug.Log(other.gameObject);
                other.gameObject.SendMessageUpwards("Die");
            }
        }
    }
}