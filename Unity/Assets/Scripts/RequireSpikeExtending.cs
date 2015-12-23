using UnityEngine;
using System.Collections;
using System;

namespace JumpyWorld {
public class RequireSpikeExtending : ICondition {


        public SpikeStateManager ssm;

        public override bool evaluate()
        {
            return ssm.currentState == SpikeStateManager.SpikeStates.Extending;
        }
    }
}