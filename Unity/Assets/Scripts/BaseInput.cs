using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class BaseInput : MonoBehaviour
	{
		public virtual IList<Touch> touches {
			get;
			set;
		}

		public virtual int touchCount {
			get {
				return touches.Count;
			}
		}
	}
}
