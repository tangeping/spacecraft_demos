using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : testbase
{

	// Use this for initialization
    public float speed = 1.0f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("current time:" + System.DateTime.Now + ",Time.deltaTime:"+ Time.deltaTime);

        Vector3 vec = transform.forward * speed * Time.deltaTime;
        Debug.Log("vec:" + vec+",before:"+transform.position);
        transform.Translate(vec);
        Debug.Log("after:"+transform.position);
        // 
        //transform.position += vec;
		
	}
}
