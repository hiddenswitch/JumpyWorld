﻿using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class InstantRevival : MonoBehaviour
	{

		// Use this for initialization
		protected virtual void OnObjectDied (GameObject sender)
		{
			var death = sender.GetComponent<Killable> ();
			if (death == null) {
				return;
			}
			death.ResetDies ();
		}
	}
}