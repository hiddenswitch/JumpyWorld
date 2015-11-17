﻿using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RespawnIfFallen : MonoBehaviour
	{
		public float respawnDepth;
		Vector3 startPosition;

		// Use this for initialization
		void Start ()
		{
			startPosition = transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (transform.position.y < respawnDepth) {
                ResetPosition();
			}
		}


        public void ResetPosition()
        {
            transform.position = startPosition;
        }
    }
}