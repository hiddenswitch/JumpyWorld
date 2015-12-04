using UnityEngine;
using System.Collections;
using System;

namespace JumpyWorld
{
    [RequireComponent (typeof (ICondition))]
    public class ConditionalCollect : Collects
    {
        public ICondition condition;

        void Start()
        {
            this.condition = condition ?? GetComponent<ICondition>();
            base.Start();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (condition.evaluate())
            {
                base.OnTriggerEnter(other);
            }
        }

    }
}