using UnityEngine;
using System.Collections;

namespace JumpyWorld {
public class PlatformConnection : MonoBehaviour {
    Platform ownerPlatform;
    Platform otherPlatform;
	// Use this for initialization
	void Start () {
        foreach (Collider other in Physics.OverlapSphere(gameObject.transform.position, 1f))
        {

            if (other.gameObject == this.gameObject)
            {
                continue;
            } else if (other.gameObject.CompareTag("platformConnection") && otherPlatform == null)
            {
                PlatformConnection otherConnection = other.gameObject.GetComponent<PlatformConnection>();
                otherPlatform = otherConnection.ownerPlatform;
                otherConnection.setOtherPlatform(this.ownerPlatform);
                
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (otherPlatform == null)
            {
                BroadcastMessage("generatePlatform", this);
            }
        }
    }
    

   //checks to avoid stuff.
    void OnTriggerStay(Collider other)
        {
            
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
}
