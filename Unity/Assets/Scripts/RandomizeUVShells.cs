using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class RandomizeUVShells : MonoBehaviour
	{
		[System.Serializable]
		public struct UVShellInfo
		{
			public int[] uvIndices;
			public Rect location;
			public bool clamp;
		}
		public MeshFilter meshFilter;
		public bool randomizeOnStart;
		public UVShellInfo[] shells;
		Vector2[] startUVs;
		Mesh mesh;
		// Use this for initialization
		void Awake ()
		{
			mesh = meshFilter.mesh;
			startUVs = mesh.uv;
			if (randomizeOnStart) {
				Randomize ();
			}
		}

		public void Randomize ()
		{
			Vector2[] newUVs = new Vector2[startUVs.Length];
			startUVs.CopyTo (newUVs, 0);
			for (var i = 0; i < shells.Length; i++) {
				var shell = shells [i];
				var displacement = Vector2.Lerp (shell.location.min, shell.location.max, Random.value) - startUVs [shell.uvIndices [0]];
				for (var j = 0; j < shell.uvIndices.Length; j++) {
					var k = shell.uvIndices [j];
					var newVal = startUVs [k] + displacement;
					if (shell.clamp) {
						newUVs [k] = new Vector2 (Mathf.Clamp (newVal.x, shell.location.xMin, shell.location.xMax), Mathf.Clamp (newVal.y, shell.location.yMin, shell.location.yMax));
					} else {
						newUVs [k] = newVal;
					}
				}
			}
			mesh.uv = newUVs;
		}
	}
}