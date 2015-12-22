using UnityEngine;
using System.Collections;

public class DieBelowDepth : MonoBehaviour {

    public float depth;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < depth)
        {
            this.gameObject.BroadcastMessage("Die");
        }
    }
}
