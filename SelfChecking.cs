using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfChecking : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (gameObject.tag == "Sugar Panel" || gameObject.tag == "Starch Panel")
        {
            if (gameObject != LabManager.LM.fn_GetInfoPanel())
            {
                gameObject.SetActive(false);
            }
        }
    }
}
