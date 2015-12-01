using UnityEngine;
using System.Collections;
using JumpyWorld;

namespace JumpyWorld.UI
{
	public class LoadingScreenController : MonoBehaviour
	{
		public Animator loadingScreenAnimator;
		public string loadingParameterName = "loading_b";
		public IsLoading isLoading;
		bool previousIsLoading;

		// Use this for initialization
		void Start ()
		{
			if (isLoading == null) {
				Debug.LogWarning ("IsLoading not set.");
				return;
			}
			previousIsLoading = isLoading.value;
			loadingScreenAnimator.SetBool (loadingParameterName, isLoading.value);
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isLoading == null) {
				return;
			}
			if (isLoading.value != previousIsLoading) {
				loadingScreenAnimator.SetBool (loadingParameterName, isLoading.value);
			}
			previousIsLoading = isLoading.value;
		}
	}
}
