using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class ObjectFollower : MonoBehaviour
	{

		public int moveSpeed = 1;
		public Transform target;
		public int rotationSpeed = 1;
		public Floor floor;
		public float stop = 0f;
		
		// Update is called once per frame
		// copied and modified from http://answers.unity3d.com/questions/26177/how-to-create-a-basic-follow-ai.html
		void Update ()
		{
			var myTransform = this.gameObject.transform;
			var distance = Vector3.Distance (myTransform.position, target.position);
			var bounds = floor.size;

			var targetInBounds = (bounds.xMin <= target.position.x) && 
				(target.position.x <= bounds.xMax) && 
				(bounds.yMin <= target.position.z) && 
				(target.position.z <= bounds.yMax);

			// if not in the bounds by the floor: sleep
			if (targetInBounds && distance > stop) {
				//move towards the player
				myTransform.rotation = Quaternion.Slerp (myTransform.rotation,
				                                        Quaternion.LookRotation (target.position - myTransform.position), rotationSpeed * Time.deltaTime);
				myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
			}			
		}
	}
}