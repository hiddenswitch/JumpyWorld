using UnityEngine;
using System.Collections;

namespace JumpyWorld { 
    public abstract class ICondition : ScriptableObject
    {
        public abstract bool evaluate();
    }
}