using UnityEngine;
using System.Collections;
using System;

namespace JumpyWorld
{
    public class ConditionalCollect : Collects
    {
        
        public Func<bool> condition;

        void receiveCondition(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (condition())
            {
                base.OnTriggerEnter(other);
            }
        }

    }
}