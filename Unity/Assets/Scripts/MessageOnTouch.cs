using UnityEngine;
using System.Collections;

public class MessageOnTouch : MonoBehaviour {

    public LayerMask triggersWith;
    public string otherTag;
    public GameObject recepient;
    public string message;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & triggersWith.value) > 0
        && other.gameObject.CompareTag(this.otherTag))
        {
            recepient.SendMessage(message, SendMessageOptions.DontRequireReceiver);
        }
    }
}
