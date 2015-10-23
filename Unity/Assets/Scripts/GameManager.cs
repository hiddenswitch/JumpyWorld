using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class GameManager : MonoBehaviour
	{
		public BoardGenerator boardScript;
		// Use this for initialization
		void Awake ()
		{
			boardScript = boardScript ?? GetComponent<BoardGenerator> ();
			InitGame ();
		}

		void InitGame ()
		{
			boardScript.SetupScene ();
		}
		// Update is called once per frame
		void Update ()
		{

		}
	}
}