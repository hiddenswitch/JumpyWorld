﻿// 3D Bresenham algorithm as an enumerable
// Based on the original code from here: http://www.luberth.com/plotter/line3d.c.txt.html
//
// I'd love to hear from you if you do anything cool with this or have any suggestions :)
// www.tenebrous.co.uk
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnifyCommunity
{
	/// <summary>
	/// Bresenham3D script from http://wiki.unity3d.com/index.php/Bresenham3D modified
	/// </summary>
	public class Bresenham3D : IEnumerable<Vector3>
	{
		Vector3 start;
		Vector3 end;
		float steps = 1;
	
		public Bresenham3D (Vector3 p_start, Vector3 p_end)
		{
			start = p_start;
			end = p_end;
			steps = 1;
		}

		public Bresenham3D (Vector3 p_start, Vector3 p_end, float p_steps)
		{
			steps = p_steps;
			start = p_start * steps;
			end = p_end * steps;
		}
	
		public IEnumerator<Vector3> GetEnumerator ()
		{
			Vector3 result;
		
			int xd, yd, zd;
			int x, y, z;
			int ax, ay, az;
			int sx, sy, sz;
			int dx, dy, dz;
		
			dx = (int)(end.x - start.x);
			dy = (int)(end.y - start.y);
			dz = (int)(end.z - start.z);
		
			ax = Mathf.Abs (dx) << 1;
			ay = Mathf.Abs (dy) << 1;
			az = Mathf.Abs (dz) << 1;
		
			sx = (int)Mathf.Sign ((float)dx);
			sy = (int)Mathf.Sign ((float)dy);
			sz = (int)Mathf.Sign ((float)dz);
		
			x = (int)start.x;
			y = (int)start.y;
			z = (int)start.z;
		
			if (ax >= Mathf.Max (ay, az)) { // x dominant
				yd = ay - (ax >> 1);
				zd = az - (ax >> 1);
				for (; ;) {
					result.x = (int)(x / steps);
					result.y = (int)(y / steps);
					result.z = (int)(z / steps);
					yield return result;
				
					if (x == (int)end.x)
						yield break;
				
					if (yd >= 0) {
						y += sy;
						yd -= ax;
					}
				
					if (zd >= 0) {
						z += sz;
						zd -= ax;
					}
				
					x += sx;
					yd += ay;
					zd += az;
				}
			} else if (ay >= Mathf.Max (ax, az)) { // y dominant
				xd = ax - (ay >> 1);
				zd = az - (ay >> 1);
				for (; ;) {
					result.x = (int)(x / steps);
					result.y = (int)(y / steps);
					result.z = (int)(z / steps);
					yield return result;
				
					if (y == (int)end.y)
						yield break;
				
					if (xd >= 0) {
						x += sx;
						xd -= ay;
					}
				
					if (zd >= 0) {
						z += sz;
						zd -= ay;
					}
				
					y += sy;
					xd += ax;
					zd += az;
				}
			} else if (az >= Mathf.Max (ax, ay)) { // z dominant
				xd = ax - (az >> 1);
				yd = ay - (az >> 1);
				for (; ;) {
					result.x = (int)(x / steps);
					result.y = (int)(y / steps);
					result.z = (int)(z / steps);
					yield return result;
				
					if (z == (int)end.z)
						yield break;
				
					if (xd >= 0) {
						x += sx;
						xd -= az;
					}
				
					if (yd >= 0) {
						y += sy;
						yd -= az;
					}
				
					z += sz;
					xd += ax;
					yd += ay;
				}
			}
		}


		private IEnumerator GetEnumerator1 ()
		{
			return this.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator1 ();
		}
	}
}