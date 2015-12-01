using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
	public class Pin<T> : MonoBehaviour, PinTrait<T>
	{
		public virtual T value {
			get;
			set;
		}
	}

	public class PinBool : Pin<bool>
	{
	}

	public interface PinTrait<T>
	{
		T value { get; set; }
	}
}
