using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	[RequireComponent(typeof(CharacterController))]
	public class MovesAlongPath : MonoBehaviour
	{
		/// <summary>
		/// Path in world space
		/// </summary>
		public Vector3[] path;
		CharacterController characterController;
		public bool shouldTeleportToStartOfPath;
		// Use this for initialization
		void Start ()
		{
			this.characterController = this.GetComponent<CharacterController> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			// Somewhere, I use the path to move
			// Somewhere, I'm keeping track of my progress along the path, etc. etc.
			// Somewhere, this happens
			var velocity = Vector3.zero;
			characterController.Move (velocity * Time.deltaTime);
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.blue;
			for (var i = 0; i < path.Length - 1; i++) {
				var p0 = path [i];
				p0.y += 0.5f;
				var p1 = path [i + 1];
				p1.y += 0.5f;
				Gizmos.DrawLine (p0, p1);
			}
		}
	}
}
