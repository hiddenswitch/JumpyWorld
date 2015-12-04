using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class AnimatorIsTransitioning : PinBool
	{
		public Animator animator;

		public override bool value {
			get {
				if (animator == null) {
					return false;
				}
				return animator.IsInTransition (0);
			}
			set {
				return;
			}
		}
	}
}