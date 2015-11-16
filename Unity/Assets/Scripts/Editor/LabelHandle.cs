using UnityEditor;
using System.Collections;
using UnityEngine;

namespace JumpyWorld
{
	[CustomEditor(typeof(RandomizeUVShells))]    
	public class LabelHandle : Editor
	{
		void OnSceneGUI ()
		{            
			Handles.BeginGUI (); 
			Handles.color = Color.blue;
		
			RandomizeUVShells cubeController = (RandomizeUVShells)target;
		
			var mesh = cubeController.gameObject.GetComponentInChildren<MeshFilter> ().sharedMesh;
			var vertices = mesh.vertices;
			var triangles = mesh.triangles;
			Debug.Log (triangles.Length);
			Debug.Log (vertices.Length);

			for (var i = 0; i < triangles.Length/3; i++) {
				var p0 = vertices [triangles [i * 3]];
				var p1 = vertices [triangles [i * 3 + 1]];
				var p2 = vertices [triangles [i * 3 + 2]]; 
				var planePoint = (1.0f / 3.0f) * (p0 + p1 + p2);
				Handles.Label (planePoint, string.Format ("{0}, {1}, {2}", i * 3, i * 3 + 1, i * 3 + 2));
			}
		
			Handles.EndGUI ();
		}
	}
}