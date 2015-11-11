using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class Hallway : Generator
	{
		[Header("Optional")]
		public Vector3
			startPoint;
		public Vector3 endPoint;
		public bool shouldBalanceCorners = false;
		[Header("Runtime")]
		public List<Vector3>
			pathBackBone;
		public List<Vector3> pathPositions = new List<Vector3> ();
        public float turbulence=2;

		// Use this for initialization
		public override void Generate (int seed)
		{
			// Create the path
			pathPositions = BresenhamFilledPath (from: startPoint, to: endPoint, shouldBalanceCorners: shouldBalanceCorners);
			pathPositions = Generator.Perturb (pathPositions,turbulence);
			pathPositions = MakePathable (pathPositions);
			// Figure out where to place the anchors based on path positions
			// If we didn't generate enough path positions for anchors, exit.
			if (pathPositions.Count < 2) {
				return;
			}

			var startAnchorPosition = PositionForAnchor (new Vector3[] {
				pathPositions [0],
				pathPositions [1]
			});
			var endAnchorPosition = PositionForAnchor (new Vector3[] {
				pathPositions [pathPositions.Count - 1],
				pathPositions [pathPositions.Count - 2]
			});

			// Setup the anchors
			anchors = new Anchor[] {
				new Anchor () {
					position = startAnchorPosition,
					directions = (startPoint - startAnchorPosition).ToDirection()
				},
				new Anchor () {
					position = endAnchorPosition,
					Directions = (endAnchorPosition - endPoint).ToDirection()
				}
			};
		}
        public override void OnDrawGizmos ()
        {
            base.OnDrawGizmos ();
            for (var i = 0; i < pathPositions.Count-1; i++) {
                Gizmos.color = Color.Lerp(Color.green, Color.yellow, (Mathf.Pow(-1.0f,i)+1.0f)/2.0f);
                Gizmos.DrawLine(pathPositions[i], pathPositions[i+1]);

            }
//            Gizmos.DrawCube(pathPositions[1],new Vector3(1,1,1));
//            Gizmos.DrawCube(startPoint,new Vector3(1,1,1));
//            Gizmos.DrawCube (endPoint,new Vector3(1,1,1));
        }
		public static Vector3 PositionForAnchor (Vector3[] tileLocations)
		{
			// Detect patterns and place the anchor where the X is located
			// X    #     
			// #    #    ##X   X##
			// #    X

			// Draw a straight line from the first two the second tiles. Then place the anchor
			// where the reverse of that line + the start tile lays.
			var tile2 = tileLocations [1] - tileLocations [0];
			return tileLocations [0] - tile2;
		}

		public override void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{
			// Draw the terrain
			for (int i = 0; i < pathPositions.Count; i++) {
				tileDrawer.DrawTerrain (tile: tilePool.defaultGround, at: pathPositions [i]);
			}
		}

		public static List<Vector3> BresenhamFilledPath (Vector3 from=default(Vector3), Vector3 to=default(Vector3), bool shouldBalanceCorners=false)
		{
			var path = new List<Vector3> ();
			// Change the origin of the bresenham to be at the start point
			var bresenham = new UnifyCommunity.Bresenham3D (Vector3.zero, to-from);
			// Detect two patterns and fill in a corner:
			// #        #
			//  #  or  #
			// Fill in any of the other corners when that pattern is detected
			Vector3[] lastTwoPoints = new Vector3[2];
			// Use a toggle value to alternate which corner gets filled
			bool toggle = false;

			foreach (var point in bresenham) {
				lastTwoPoints [0] = lastTwoPoints [1];
				lastTwoPoints [1] = point;
				// Detect if a diagonal pattern occurred
				// TODO: Make this work for 3D
				if (lastTwoPoints [0].z != lastTwoPoints [1].z && lastTwoPoints [0].x != lastTwoPoints [1].x) {
					// Fill in a corner point
					var cornerPoint = Vector3.zero;
                    print ("diagonal");
                    print (lastTwoPoints[0]);

					if (toggle) {
						cornerPoint.x = Mathf.Min (lastTwoPoints [0].x, lastTwoPoints [1].x);
						cornerPoint.z = Mathf.Min (lastTwoPoints [0].z, lastTwoPoints [1].z);
					} else {
						cornerPoint.x = Mathf.Max (lastTwoPoints [0].x, lastTwoPoints [1].x);
						cornerPoint.z = Mathf.Max (lastTwoPoints [0].z, lastTwoPoints [1].z);
					}
                    if (cornerPoint==lastTwoPoints[0] || cornerPoint==lastTwoPoints[1]){
                        if (toggle) {
                            cornerPoint.x = Mathf.Min (lastTwoPoints [0].x, lastTwoPoints [1].x);
                            cornerPoint.z = Mathf.Max (lastTwoPoints [0].z, lastTwoPoints [1].z);
                        } else {
                            cornerPoint.x = Mathf.Max (lastTwoPoints [0].x, lastTwoPoints [1].x);
                            cornerPoint.z = Mathf.Min (lastTwoPoints [0].z, lastTwoPoints [1].z);
                        }
                    }   
					if (shouldBalanceCorners) {
						toggle = !toggle;
					}
					path.Add (cornerPoint+from);
				}
				path.Add (point+from);
			}

			return path;
		}
	}
}
