using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcollider : MonoBehaviour {

    // Use this for initialization
    public float froce = 1900;
    public float speed = 100;

    Rigidbody rb = null;

    void Start () {

        rb = GetComponent<Rigidbody>();
        Debug.Log("speed:" + speed);
        //       rb.AddForce(transform.forward * froce);
        rb.velocity = transform.forward * speed;
    }
	
	// Update is called once per frame
	void Update () {

        //transform.position += transform.forward * speed * Time.deltaTime;
	}

    

//     void FixedUpdate()
//     {
// 
//         
// 
//     }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter other:" + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit other:" + other.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter other:" + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit other:" + collision.gameObject.name);
    }
}
