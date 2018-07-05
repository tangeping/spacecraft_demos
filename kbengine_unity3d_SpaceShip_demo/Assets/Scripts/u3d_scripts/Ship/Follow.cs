using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private Transform _target;            // The position that that camera will be following.
    private float smoothing = 50f;        // The speed with which the camera will be following.

    Vector3 offset = new Vector3(0.0f, 2.3f, -5.0f);                     // The initial offset from the target.

    //     [HideInInspector] float Distance = 1.9f;//主相机与目标物体之间的距离 
    // 
    //     [HideInInspector] float Height = 0.35f;

    public void AttachTarget(Transform target)
    {
        _target = target;

        //         offset = transform.position - target.position;
        // 
        //         Debug.Log("Follow::AttachTarget,offset:" + offset 
    }

    private void FixedUpdate()
    {
        if(_target == null)
        {
            return;
        }

        Vector3 targetCamPos = _target.position + offset;
        

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    //     public void FllownTarge(Transform target)
    //     {
    //         if (target != null)
    //         {
    //             Quaternion quaternion = Quaternion.Euler(0.0f, target.eulerAngles.y, .0f);
    //             Vector3 vector = (quaternion * new Vector3(.0f, Height, -Distance)) + target.position;
    // 
    //             transform.rotation = quaternion;
    //             transform.position = vector;
    //             transform.parent = target;
    //         }
    //    }
}
