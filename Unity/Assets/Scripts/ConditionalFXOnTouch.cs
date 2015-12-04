using UnityEngine;
using System.Collections;

namespace JumpyWorld { 
    [RequireComponent (typeof (ICondition))]
    public class ConditionalFXOnTouch : FXOnTouch {

        public ICondition condition;
        public override void OnTriggerEnter(Collider other)
        {
            if (condition.evaluate())
            {
                base.OnTriggerEnter(other);
            }
        }
    }
}
