using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class Floor : Generator
	{
		public struct RectanglePoint
		{
			public Vector3 position;
			public Directions side;
			public bool isBorder;
			/// <summary>
			/// The distance from {North, East, South, West} borders
			/// </summary>
			public Vector4 distanceFromBorders;
			public Vector4 distanceFromCenter;
		}

		[Header("Options")]
		public Rect
			size;
		public bool shouldGenerateAnchors = true;

		public override Bounds BoundsGrid {
			get {
				var center = size.center;
				var rectSize = size.size;
				return new Bounds (new Vector3 (center.x, 0f, center.y), new Vector3 (rectSize.x, 0f, rectSize.y));
			}
			set {
				size.min = new Vector2 (value.min.x, value.min.z);
				size.size = new Vector2 (value.size.x, value.size.z); 
			}
		}

		public override void Generate (int seed)
		{
			if (!shouldGenerateAnchors) {
				anchors = new Anchor[] {};
				return;
			}
			// Compute how far along the border we should place the anchor.
			// We will kill a dictionary entry as soon as we consume it (as soon as we place
			// the anchor for that (border, position) tuple.
			var anchorPositions = new Dictionary<Directions, float> () {
				{Directions.North, Random.Range (0.1f, 0.9f)},
				{Directions.South, Random.Range (0.1f, 0.9f)},
				{Directions.East, Random.Range (0.1f, 0.9f)},
				{Directions.West, Random.Range (0.1f, 0.9f)}
			};

			// Use these bounds to compute progress along a border
			var bounds = BoundsGrid;
			// A temporary list to store the anchors
			var anchorList = new List<Anchor> ();

			foreach (var info in Rectangle(rect: size)) {
				// Should we put an anchor here?
				if (info.isBorder) {
					// Determine what axis we're working with based on the direction of this border.
					// If it is an east or west border, we'll be interested in our progress along the z axis, indicated by 2.
					// Otherwise, we are interested in our progress along the x axis, indicated by zero.
					var axis = ((info.side & Directions.East) == Directions.East) ||
						((info.side & Directions.West) == Directions.West) ? 2 : 0;
					// Now, use the axis value to determine our progress along the border. 
					var progressAlongBorder = Mathf.InverseLerp (bounds.min [axis], bounds.min [axis] + bounds.size [axis], info.position [axis]);
					if (anchorPositions.ContainsKey (info.side) &&
					// This will be true the first moment the progress along the border is larger
					// than the amount indicated by anchor positions.
						progressAlongBorder > anchorPositions [info.side]) {
						// Compute the placement of the anchor based on the side (should be one off from the border)
						var displacement = info.side.ToVector ();

						// Place anchor
						anchorList.Add (new Anchor () {
                            PositionGrid = info.position + displacement,
                            Directions = info.side,
						});
						// Consume the anchor.
						anchorPositions.Remove (info.side);
					}
				}
			}

			anchors = anchorList.ToArray ();
		}

		public override void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{
			foreach (var info in Rectangle(rect: size)) {
				tileDrawer.DrawTerrain (tilePool.defaultGround, at: info.position, isDynamic: false);
			}
		}

		public static IEnumerable<RectanglePoint> Rectangle (Rect rect, float step=1.0f, float y=0f)
		{
			for (var x = rect.xMin; x <= rect.xMax; x+=step) {
				for (var z = rect.yMin; z <= rect.yMax; z+=step) {
					Directions side = Directions.None;
					bool isBorder = x == rect.xMin
						|| x == rect.xMax 
						|| z == rect.yMin
						|| z == rect.yMax;

					if (isBorder) {
						if (x == rect.xMin) {
							side = Directions.West;
						} else if (x == rect.xMax) {
							side = Directions.East;
						} else if (z == rect.yMin) {
							side = Directions.South;
						} else if (z == rect.yMax) {
							side = Directions.North;
						}
					} else {
						if (x >= rect.xMin && x < rect.center.x) {
							side |= Directions.West;
						} else if (x > rect.center.x && x <= rect.xMax) {
							side |= Directions.East;
						}
						
						if (z >= rect.yMin && z < rect.center.y) {
							side |= Directions.South;
						} else if (z > rect.center.y && z <= rect.yMax) {
							side |= Directions.North;
						}
					}

					var distanceFromBorders = new Vector4 (Mathf.Abs (rect.yMax - z), Mathf.Abs (x - rect.xMax), Mathf.Abs (z - rect.yMin), Mathf.Abs (x - rect.xMin));

					var distanceFromCenter = new Vector4 (Mathf.Abs (rect.center.y - z), Mathf.Abs (x - rect.center.x), Mathf.Abs (z - rect.center.y), Mathf.Abs (x - rect.center.x));

					var point = new RectanglePoint () {
						position = new Vector3 (x, y, z),
						side = side,
						isBorder = isBorder,
						distanceFromBorders = distanceFromBorders,
						distanceFromCenter = distanceFromCenter
					};

					yield return point;
				}
			}
		}
	}
}
