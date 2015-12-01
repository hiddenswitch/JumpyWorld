using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class BoolOr : PinBool
	{
		public PinBool left;
		public PinBool right;
		public override bool value {
			get {
				return left.value || right.value;
			}
			set {
				return;
			}
		}
	}
}
