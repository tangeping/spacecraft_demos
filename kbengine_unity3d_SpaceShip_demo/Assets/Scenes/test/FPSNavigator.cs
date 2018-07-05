using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSNavigator : MonoBehaviour {

    public KeyCode mKeyLeft = KeyCode.LeftArrow;
    public KeyCode mKeyRight = KeyCode.RightArrow;
    public KeyCode mKeyForward = KeyCode.UpArrow;
    public KeyCode mKeyBackward = KeyCode.DownArrow;

    public float mKeyStrokeMoveStep = 0.07f;    //metre  

    private CharacterController controller;
    private Vector3 mMoveDir;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 vDir = Vector3.zero;
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
        mMoveDir = transform.rotation * vDir;

        controller.Move(mMoveDir);
    }
}
