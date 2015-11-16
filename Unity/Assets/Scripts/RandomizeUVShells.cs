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
		}
		public MeshFilter meshFilter;
		public bool randomizeOnStart;
		public UVShellInfo[] shells;

		public Vector2[] startUVs;
		Mesh mesh;
		// Use this for initialization
		void Start ()
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
					newUVs [k] = startUVs [k] + displacement;
				}
			}
			mesh.uv = newUVs;
		}
	}
}