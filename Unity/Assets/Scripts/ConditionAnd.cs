using UnityEngine;
using System.Collections;
using System;

namespace JumpyWorld { 
    public class ConditionAnd : ICondition {
        public ICondition condition1;
        public ICondition condition2;

        
        public override bool evaluate()
        {
            return condition1.evaluate() && condition2.evaluate();
        }

    }
}