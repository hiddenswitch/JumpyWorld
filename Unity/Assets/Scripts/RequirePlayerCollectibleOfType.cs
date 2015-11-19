using UnityEngine;
using System.Collections;
using System;

public class RequirePlayerCollectibleOfType : MonoBehaviour {
    public string collectibleSavePrefix = "Collectible.";
    public string collectibleTag;
    // Use this for initialization
    void Start () {
        Func<bool> requireObject = () =>
        {
            return PlayerPrefs.GetInt(collectibleSavePrefix + collectibleTag) >= 1;
        };

        this.SendMessage("receiveCondition", requireObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
