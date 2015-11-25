using UnityEngine;
using System.Collections;
using UnityEditor;

namespace JumpyWorld { 
    public abstract class ICondition : ScriptableObject
    {
        public abstract bool evaluate();
    }
}