using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[ExecuteInEditMode()]
	public class AdjustCameraFOV : MonoBehaviour
	{
		public float landscapeFOV;
		public float portraitFOV;
		public new Camera camera;
		float previousFOV;
		// Use this for initialization
		void Start ()
		{
			camera = camera ?? GetComponent<Camera> () ?? Camera.main;
			previousFOV = camera.fieldOfView;
		}
	
		// Update is called once per frame
		void Update ()
		{
#if UNITY_EDITOR
			if (Screen.width > Screen.height) {
				if (previousFOV != landscapeFOV) {
					camera.fieldOfView = previousFOV = landscapeFOV;
				}
			} else {
				if (previousFOV != portraitFOV) {
					camera.fieldOfView = previousFOV = portraitFOV;
				}
			}
#else
			switch (Screen.orientation) {
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				if (previousFOV != landscapeFOV) {
					camera.fieldOfView = previousFOV = landscapeFOV;
				}
				break;
			default:
				if (previousFOV != portraitFOV) {
					camera.fieldOfView = previousFOV = portraitFOV;
				}
				break;
			}
#endif
		}
	}

}