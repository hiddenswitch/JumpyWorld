using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[ExecuteInEditMode()]
	public class AdjustCameraOrthographicSize : MonoBehaviour
	{
		public float landscapeSize;
		public float portraitSize;
		public new Camera camera;
		float previousSize;
		// Use this for initialization
		void Start ()
		{
			camera = camera ?? GetComponent<Camera> () ?? Camera.main;
			previousSize = camera.orthographicSize;
		}
	
		// Update is called once per frame
		void Update ()
		{
#if UNITY_EDITOR
			if (Screen.width > Screen.height) {
				if (previousSize != landscapeSize) {
					camera.orthographicSize = previousSize = landscapeSize;
				}
			} else {
				if (previousSize != portraitSize) {
					camera.orthographicSize = previousSize = portraitSize;
				}
			}
#else
			switch (Screen.orientation) {
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				if (previousSize != landscapeSize) {
					camera.orthographicSize = previousSize = landscapeSize;
				}
				break;
			default:
				if (previousSize != portraitSize) {
					camera.orthographicSize = previousSize = portraitSize;
				}
				break;
			}
#endif
		}
	}

}