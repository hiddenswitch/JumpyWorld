using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JumpyWorld{
	[RequireComponent (typeof(Image))]
	public class HideOnWorldGenerationCompleted : MonoBehaviour {
		Image image;

		public int fadeFrames = 30;
		bool kill = false;
		// Use this for initialization
		void Start () {
			image = gameObject.GetComponent<Image> ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		void WorldGenerationStarted(){
			kill = true;
		}
		
		void WorldGenerationFinished(){
			kill = false;
		}
		

		
	}
}