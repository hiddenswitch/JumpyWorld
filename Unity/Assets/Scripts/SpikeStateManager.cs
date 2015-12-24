using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class SpikeStateManager : MonoBehaviour
	{

        
		public float retractedTime;
		public float extendingTime;
		public float extendedTime;
		public float retractingTime;

        
		public enum SpikeStates
		{
			Retracted,
			Extending,
			Extended,
			Retracting}

		;

		public Dictionary<SpikeStates, float> spikeStateTimes = new Dictionary<SpikeStates, float> ();

		public SpikeStates currentState = SpikeStates.Retracted;
		public float timeRemainingInCurrentState;

		// Use this for initialization
		void Start ()
		{
			spikeStateTimes.Add (SpikeStates.Retracted, retractedTime);
			spikeStateTimes.Add (SpikeStates.Extending, extendingTime);
			spikeStateTimes.Add (SpikeStates.Extended, extendedTime);
			spikeStateTimes.Add (SpikeStates.Retracting, retractingTime);
			timeRemainingInCurrentState = Random.Range (0f, spikeStateTimes [currentState]);
		}
	
		// Update is called once per frame
		void Update ()
		{
			timeRemainingInCurrentState -= Time.deltaTime;
			if (timeRemainingInCurrentState < 0) {
				currentState += 1;
				currentState = (SpikeStates)(((int)currentState) % 4);
				timeRemainingInCurrentState = spikeStateTimes [currentState];
			}

		}
	}
}