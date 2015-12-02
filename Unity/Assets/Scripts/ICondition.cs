using UnityEngine;
using System.Collections;

namespace JumpyWorld { 
    public abstract class ICondition : MonoBehaviour
    {
        public abstract bool evaluate();
    }
}