using UnityEngine;
using System.Collections;
namespace JumpyWorld
{ 
[RequireComponent(typeof( Rigidbody))]
public class ResetVelocityOnDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnObjectDied(GameObject sender)
    {
        if (enabled)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
}