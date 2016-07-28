using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ChangeCameraPOV : MonoBehaviour
	{
		public GameObject isometricCamera;
		public GameObject topDownCamera;

		// Use this for initialization
		void Start ()
		{
			UpdateCamera ();
		}

		void UpdateCamera ()
		{
			if (Application.isMobilePlatform) {
				isometricCamera.SetActive (true);
				topDownCamera.SetActive (false);
			} else {
				topDownCamera.SetActive (true);
				isometricCamera.SetActive (false);
			}
		}
	}
}