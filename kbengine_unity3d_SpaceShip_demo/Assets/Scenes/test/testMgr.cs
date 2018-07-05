using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {

        testbase test = GetComponent<testbase>();
        Debug.Log("test:" + test);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
