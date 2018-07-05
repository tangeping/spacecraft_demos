using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test6 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Follow followScript = Camera.main.GetComponent<Follow>();

        followScript.AttachTarget(transform,Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
