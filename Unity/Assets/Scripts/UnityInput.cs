using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class UnityInput : BaseInput
	{
		public override int touchCount {
			get {
				return Input.touchCount;
			}
		}

		public override IList<Touch> touches {
			get {
				return Input.touches;
			}
			private set {
				return;
			}
		}
	}
}