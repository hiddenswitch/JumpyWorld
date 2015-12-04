using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using JumpyWorld;

namespace JumpyWorld.UI
{
	public class RenderCollects : MonoBehaviour
	{
		public Text text;
		public Collects collects;
		public string formatter = "{0}";
		// Update is called once per frame
		void Update ()
		{
			text.text = String.Format (formatter, collects.count);
		}
	}
}