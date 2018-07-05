using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private Transform _target;            // The position that that camera will be following.
    private float smoothing = 50f;        // The speed with which the camera will be following.

    Vector3 offset = Vector3.zero;  // The initial offset from the target.

    public void AttachTarget(Transform target,Vector3 carmOffset)
    {
        _target = target;

        offset = carmOffset /*transform.position - target.position*/;

        Debug.Log("Follow::AttachTarget,offset:" + offset);
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

}
