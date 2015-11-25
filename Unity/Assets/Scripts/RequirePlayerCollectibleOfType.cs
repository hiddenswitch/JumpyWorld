using UnityEngine;
using System.Collections;
using System;

namespace JumpyWorld
{
    public class RequirePlayerCollectibleOfType : ICondition
    {
        public string collectibleSavePrefix = "Collectible.";
        public string collectibleTag;

        public override bool evaluate()
        {
            return PlayerPrefs.GetInt(collectibleSavePrefix + collectibleTag) >= 1;
        }
    }
}
