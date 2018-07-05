using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {

    public PropAttri _atrri = null;

    protected Ship _target = null;

    protected void AttachTarget(Ship target)
    {
        if(target == null|| _target != null)
        {
            return;
        }
        _target = target;
    }
    protected void RemoveFromTarget()
    {
        _target = null;
    }

    protected virtual void AddEffect()
    {

    }
    protected virtual void Cast()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("ship"))
        {
            return;
        }
        Debug.Log("OnCollisionEnter me:" + gameObject.name + ",other:" + collision.gameObject.name);

        AttachTarget(collision.gameObject.GetComponent<Ship>());
        Cast();
        AddEffect();
        Destroy(gameObject);
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit me:" + gameObject.name + ",other:" + collision.gameObject.name);
        RemoveFromTarget();
    }

    private void Awake()
    {
        if (_atrri != null)
        {
            transform.position = _atrri.Position;
            transform.eulerAngles = _atrri.Direction;
        }
    }
}
