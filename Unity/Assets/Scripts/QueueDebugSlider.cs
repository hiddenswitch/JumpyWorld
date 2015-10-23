using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class QueueDebugSlider : MonoBehaviour
	{
		public DelayedTurnController delayedTurnController;
		float enterSlider;
		float exitSlider;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnGUI ()
		{
			enterSlider = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), enterSlider, 0.0F, 10.0F);
			exitSlider = GUI.HorizontalSlider (new Rect (25, 55, 100, 30), exitSlider, 0.0F, 10.0F);

			delayedTurnController.enterCellThreshold = (-1) * enterSlider * (0.05f);
			delayedTurnController.exitCellThreshold = exitSlider * 0.05f;
		}
	}
}