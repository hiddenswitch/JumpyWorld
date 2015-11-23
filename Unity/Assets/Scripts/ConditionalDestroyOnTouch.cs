using UnityEngine;
using System.Collections;
using System;

namespace JumpyWorld
{
    public class ConditionalDestroyOnTouch : DestroyOnTouch
    {
        public ICondition condition;

        public override void OnTriggerEnter(Collider other) {
            if (condition.evaluate())
            {
                base.OnTriggerEnter(other);
            }
        }
    }
}