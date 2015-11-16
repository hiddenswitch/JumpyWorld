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

			for (var i = 0; i < triangles.Length; i+=3) {
				var p0 = vertices [triangles [i]];
				var p1 = vertices [triangles [i + 1]];
				var p2 = vertices [triangles [i + 2]]; 

				var planePoint = Vector3.zero;
				var planeNormal = Vector3.zero;
				PlaneFrom3Points (out planeNormal, out planePoint, p0, p1, p2);

				planePoint = cubeController.transform.TransformVector (planePoint);
				planeNormal = cubeController.transform.TransformDirection (planeNormal);

				Handles.Label (planePoint, string.Format ("{0}, {1}, {2}", triangles [i], triangles [i + 1], triangles [i + 2]));
				Handles.DrawLine (planePoint, planePoint + planeNormal);
			}

			Handles.EndGUI ();
		}

		//Two non-parallel lines which may or may not touch each other have a point on each line which are closest
		//to each other. This function finds those two points. If the lines are not parallel, the function 
		//outputs true, otherwise false.
		public static bool ClosestPointsOnTwoLines (out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
		{
			
			closestPointLine1 = Vector3.zero;
			closestPointLine2 = Vector3.zero;
			
			float a = Vector3.Dot (lineVec1, lineVec1);
			float b = Vector3.Dot (lineVec1, lineVec2);
			float e = Vector3.Dot (lineVec2, lineVec2);
			
			float d = a * e - b * b;
			
			//lines are not parallel
			if (d != 0.0f) {
				
				Vector3 r = linePoint1 - linePoint2;
				float c = Vector3.Dot (lineVec1, r);
				float f = Vector3.Dot (lineVec2, r);
				
				float s = (b * f - c * e) / d;
				float t = (a * f - c * b) / d;
				
				closestPointLine1 = linePoint1 + lineVec1 * s;
				closestPointLine2 = linePoint2 + lineVec2 * t;
				
				return true;
			} else {
				return false;
			}
		}

		public static void PlaneFrom3Points (out Vector3 planeNormal, out Vector3 planePoint, Vector3 pointA, Vector3 pointB, Vector3 pointC)
		{
			
			planeNormal = Vector3.zero;
			planePoint = Vector3.zero;
			
			//Make two vectors from the 3 input points, originating from point A
			Vector3 AB = pointB - pointA;
			Vector3 AC = pointC - pointA;
			
			//Calculate the normal
			planeNormal = Vector3.Normalize (Vector3.Cross (AB, AC));
			
			//Get the points in the middle AB and AC
			Vector3 middleAB = pointA + (AB / 2.0f);
			Vector3 middleAC = pointA + (AC / 2.0f);
			
			//Get vectors from the middle of AB and AC to the point which is not on that line.
			Vector3 middleABtoC = pointC - middleAB;
			Vector3 middleACtoB = pointB - middleAC;
			
			//Calculate the intersection between the two lines. This will be the center 
			//of the triangle defined by the 3 points.
			//We could use LineLineIntersection instead of ClosestPointsOnTwoLines but due to rounding errors 
			//this sometimes doesn't work.
			Vector3 temp;
			ClosestPointsOnTwoLines (out planePoint, out temp, middleAB, middleABtoC, middleAC, middleACtoB);
		}
	}
}