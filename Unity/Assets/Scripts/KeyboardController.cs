using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class KeyboardController : MonoBehaviour
	{
		[Header ("Controls")]
		public KeyCode north = KeyCode.UpArrow;
		public KeyCode south = KeyCode.DownArrow;
		public KeyCode east = KeyCode.RightArrow;
		public KeyCode west = KeyCode.LeftArrow;
		public KeyCode north2 = KeyCode.W;
		public KeyCode south2 = KeyCode.S;
		public KeyCode east2 = KeyCode.D;
		public KeyCode west2 = KeyCode.A;
		[Header ("Events")]
		public string
			swipeNorth = "SwipeNorth";
		public string swipeSouth = "SwipeSouth";
		public string swipeEast = "SwipeEast";
		public string swipeWest = "SwipeWest";
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown (north) || Input.GetKeyDown (north2)) {
				BroadcastMessage (swipeNorth);
			} else if (Input.GetKeyDown (south) || Input.GetKeyDown (south2)) {
				BroadcastMessage (swipeSouth);
			} else if (Input.GetKeyDown (east) || Input.GetKeyDown (east2)) {
				BroadcastMessage (swipeEast);
			} else if (Input.GetKeyDown (west) || Input.GetKeyDown (west2)) {
				BroadcastMessage (swipeWest);
			}
		}
	}
}