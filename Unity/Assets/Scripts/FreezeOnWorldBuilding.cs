using UnityEngine;
using System.Collections;

namespace JumpyWorld{
	[RequireComponent(typeof(Rigidbody))]
	public class FreezeOnWorldBuilding : MonoBehaviour {
		public TileDrawer tileDrawer;
		Rigidbody rb;

		// Use this for initialization
		void Start () {
			if (tileDrawer == null) {
				this.enabled = false;
			}
			rb = gameObject.GetComponent<Rigidbody> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (tileDrawer.isDrawingTiles && !rb.isKinematic) {
				rb.isKinematic = true;
			} else if (rb.isKinematic) {
				rb.isKinematic = false;
			}
		}
	}
}
