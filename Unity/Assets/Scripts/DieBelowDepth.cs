using UnityEngine;
using System.Collections;

public class DieBelowDepth : MonoBehaviour
{
	public float depth;
	bool calledDie;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.y < depth
		          && !calledDie) {
			calledDie = true;
			this.gameObject.BroadcastMessage ("Die");
		}

		if (transform.position.y > depth
		    && calledDie) {
			calledDie = false;
		}
	}
}
