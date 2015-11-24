using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace JumpyWorld{
public class MoveInOnWorldGeneration : MonoBehaviour {
	
		public int inX;
		public int outX;
		public int FadeInFrames = 1;
		public int FadeOutFrames = 15;

		public AnimationCurve smoothCurve;


		public int DelayResponseFrames = 5;
		int inDelay = 0;
		int outDelay = 0;
		public TileDrawer tileDrawer;
		bool fadingIn = false;
		bool fadingOut = false;
		bool fadedIn = false;
		bool fadedOut = false;
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
			if (tileDrawer.isDrawingTiles) {
				Debug.Log ("is drawing");
				inDelay ++;
				outDelay = 0;
				if ((!fadedIn) && (inDelay >= DelayResponseFrames)){
					fadingIn = true;
					fadedOut = false;
					fadingOut = false;
					StartCoroutine(FadeIn ());
				}
			} else {
				Debug.Log ("not drawing");
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
				gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((1 - smoothCurve.Evaluate( (float) i / FadeInFrames)) * inX, 0);
				yield return new WaitForEndOfFrame();
			}
			fadingIn = false;
			fadedIn = true;
		}
		
		IEnumerator FadeOut(){
			for (int i = 0; i < FadeOutFrames + 1; i ++) {
				if (fadingIn) break;
				gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((smoothCurve.Evaluate( (float) i / FadeInFrames)) * outX, 0);

				yield return new WaitForEndOfFrame();
			}
			fadingOut = false;
			fadedOut = true;
		}
}
}