using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JumpyWorld{
	[RequireComponent (typeof(Image))]
	public class ShowOnWorldBuilding : MonoBehaviour {
		Image image;
		public int FadeInFrames = 1;
		public int FadeOutFrames = 15;
		public int DelayResponseFrames = 5;
		int inDelay = 0;
		int outDelay = 0;
		public TileDrawer tileDrawer;
		bool fadingIn = false;
		bool fadingOut = false;
		bool fadedIn = true;
		bool fadedOut = false;
		void Start () {
			image = gameObject.GetComponent<Image> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (tileDrawer.isDrawingTiles) {
				inDelay ++;
				outDelay = 0;
				if ((!fadedIn) && (inDelay >= DelayResponseFrames)){
					fadingIn = true;
					fadedOut = false;
					fadingOut = false;
					StartCoroutine(FadeIn ());
				}
			} else {
				outDelay ++ ;
				inDelay = 0;
				if (!fadedOut && (outDelay >= DelayResponseFrames)){
					fadingOut = true;
					fadedIn = false;
					fadingIn = false;
					StartCoroutine (FadeOut());
				}
			}
		}


		IEnumerator FadeIn(){
			for (int i = 0; i < FadeInFrames + 1; i ++) {
				if (fadingOut) break;
				image.color = new Color(image.color.r, image.color.g, image.color.b, (float) i / FadeInFrames);
				yield return new WaitForEndOfFrame();
			}
			fadingIn = false;
			fadedIn = true;
		}

		IEnumerator FadeOut(){
			for (int i = 0; i < FadeOutFrames + 1; i ++) {
				if (fadingIn) break;

				image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - (float) i / FadeOutFrames);
				yield return new WaitForEndOfFrame();
			}
			fadingOut = false;
			fadedOut = true;
		}


	}
}