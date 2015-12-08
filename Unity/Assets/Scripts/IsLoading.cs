using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class IsLoading : PinBool
	{
		public PinBool worldBuildingBool;
		public PinBool lifeBool;
		bool warned;

		public bool isLoading {
			get {
				if (worldBuildingBool == null
					|| lifeBool == null) {
					ShowWarningOnce ();
					return false;
				}
				return worldBuildingBool.value || !lifeBool.value;
			}
		}

		public override bool value {
			get {
				return isLoading;
			}
			set {
				return;
			}
		}

		void ShowWarningOnce ()
		{
			if (warned) {
				return;
			}

			Debug.LogWarning ("IsLoading called without setting all required dependencies. Gracefully assuming not loading.");

			warned = true;
		}
	}
}