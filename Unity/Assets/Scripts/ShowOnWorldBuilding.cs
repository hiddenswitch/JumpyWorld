using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JumpyWorld
{
	[RequireComponent (typeof(Image))]
	public class ShowOnWorldBuilding : MonoBehaviour
	{
		Image image;
		public float fadeInSeconds = 0.25f;
		public float fadeOutSeconds = 0.25f;
		public float delayResponseSeconds = 0.125f;
		float inDelay = 0;
		float outDelay = 0;
		public TileDrawer tileDrawer;
		bool fadingIn = false;
		bool fadingOut = false;
		bool fadedIn = true;
		bool fadedOut = false;
		Color opaque;
		Color transparent;

		void Start ()
		{
			image = gameObject.GetComponent<Image> ();
			opaque = image.color;
			transparent = new Color (image.color.r, image.color.g, image.color.b, 0);
		}
		
		// Update is called once per frame
		void Update ()
		{
			if (tileDrawer.isDrawingTiles) {
				inDelay += Time.deltaTime;
				outDelay = 0;
				if ((!fadedIn) && (inDelay >= delayResponseSeconds)) {
					fadingIn = true;
					fadedOut = false;
					fadingOut = false;
					StartCoroutine (FadeIn ());
				}
			} else {
				outDelay += Time.deltaTime;
				inDelay = 0;
				if (!fadedOut && (outDelay >= delayResponseSeconds)) {
					fadingOut = true;
					fadedIn = false;
					fadingIn = false;
					StartCoroutine (FadeOut ());
				}
			}
		}

		IEnumerator FadeIn ()
		{
			var time = 0f;
			while (time < fadeInSeconds) {
				if (fadingOut) {
					break;
				}
				time += Time.deltaTime;
				image.color = Color.Lerp (transparent, opaque, time / fadeInSeconds);
				yield return new WaitForEndOfFrame ();
			}

			fadingIn = false;
			fadedIn = true;
		}

		IEnumerator FadeOut ()
		{
			var time = 0f;
			while (time < fadeInSeconds) {
				if (fadingIn) {
					break;
				}
				time += Time.deltaTime;
				image.color = Color.Lerp (opaque, transparent, time / fadeInSeconds);
				yield return new WaitForEndOfFrame ();
			}
			
			fadingOut = false;
			fadedOut = true;
		}


	}
}