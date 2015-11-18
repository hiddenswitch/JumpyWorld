using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class Collects : MonoBehaviour
	{
		public enum LifetimeTypes
		{
			PerRun,
			PerUser
		}
		public string collectibleSavePrefix = "Collectible.";
		public string collectibleTag;
		public LayerMask triggersWith;
		public int count;
		public LifetimeTypes lifetime = LifetimeTypes.PerRun;
		// Use this for initialization
		void Start ()
		{
			if (lifetime == LifetimeTypes.PerUser) {
				count = PlayerPrefs.GetInt (collectibleSavePrefix + collectibleTag);
			}
		}

		/// <summary>
		/// Called by falling off, for example
		/// </summary>
		void Reset ()
		{
			if (lifetime == LifetimeTypes.PerRun) {
				count = 0;
			}
		}

		void OnTriggerEnter (Collider other)
		{
			if (((1 << other.gameObject.layer) & triggersWith.value) > 0
				&& other.gameObject.CompareTag (this.collectibleTag)) {
				count += 1;
				PlayerPrefs.SetInt (collectibleSavePrefix + collectibleTag, count);
				// record coin balance to Mixpanel 
				Mixpanel.SuperProperties["Coin Balance"] = count;
				Mixpanel.SendEvent ("Item Collected", new Dictionary<string, object>{
					{"Type", "Coin" },
					{"Count", count}
				});
			}
		}
	}
}