using UnityEngine;
using System.Collections;

namespace Clouds
{
	public class CloudParallax : MonoBehaviour
	{
		public GameObject targetCamera;
		public float moveRatio;
		Vector3 previousPosition;


		// Use this for initialization
		void Start ()
		{
			previousPosition = targetCamera.transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
			transform.position += moveRatio * (targetCamera.transform.position - previousPosition);
			previousPosition = targetCamera.transform.position;
		}
	}
}
