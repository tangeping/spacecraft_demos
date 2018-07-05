using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class keyMove : MonoBehaviour
{

    private KeyCode mKeyLeft = KeyCode.A;
    private KeyCode mKeyRight = KeyCode.D;
    private KeyCode mKeyForward = KeyCode.W;
    private KeyCode mKeyBackward = KeyCode.S;

    public float mKeyStrokeMoveStep = 0.07f;    //metre  

  //  private Rigidbody _rb;
 //   private Vector3 mMoveDir;
    public Vector3 speed = Vector3.zero;

    // Use this for initialization
    void Start()
    {
 //       _rb = GetComponent<Rigidbody>();

        gameObject.AddComponent<Follow>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vDir = Vector3.zero;
        Vector3 vDirection = Vector3.zero;

        if (Input.GetKey(mKeyLeft))
        {
            vDir.x -= mKeyStrokeMoveStep;
        }
        if (Input.GetKey(mKeyRight))
        {
            vDir.x += mKeyStrokeMoveStep;
        }

        if (Input.GetKey(mKeyForward))
        {
            vDir.z += mKeyStrokeMoveStep;
        }
        if (Input.GetKey(mKeyBackward))
        {
            vDir.z -= mKeyStrokeMoveStep;
        }

//        mMoveDir = transform.rotation * vDir;

        if (vDir != Vector3.zero)
        {
            //           _rb.velocity += mMoveDir;
            //           speed = _rb.velocity;
            transform.position += vDir;
        }

        if(vDirection != Vector3.zero)
        {
 //           _rb.MoveRotation(_rb.rotation *Quaternion.Euler(vDirection));
        }
    }
}
