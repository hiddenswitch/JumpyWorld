using UnityEngine;
using System.Collections;

public class PlatformConnection : MonoBehaviour {
    Platform ownerPlatform;
    Platform otherPlatform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(otherPlatform);
            if (otherPlatform == null)
            {
                BroadcastMessage("generatePlatform", this);
            }
        }
    }

    public void setOtherPlatform(Platform p)
    {
        otherPlatform = p;
    }

    public void setOwnerPlatform(Platform p)
    {
        ownerPlatform = p;
    }
}
