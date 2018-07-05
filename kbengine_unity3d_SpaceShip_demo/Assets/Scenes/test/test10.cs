using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test10 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string path = Application.dataPath;
        int num = path.LastIndexOf("/");
        path = path.Substring(0, num);

        Debug.Log("path:" + path);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
