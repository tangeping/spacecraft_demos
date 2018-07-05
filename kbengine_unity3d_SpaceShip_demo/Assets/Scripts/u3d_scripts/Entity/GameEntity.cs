using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour {

    public bool isPlayer = false;

 //   protected Vector3 _position = Vector3.zero;
 //   protected Vector3 _eulerAngles = Vector3.zero;
    protected Vector3 _scale = Vector3.zero;
    protected UInt32 _spaceID = 0;

    public Vector3 destPosition = Vector3.zero;
    public Vector3 destDirection = Vector3.zero;

    protected float _speed = 0f;
    private float cruiseSpeed = 0f;
    protected float jumpState = 0;


    public string entity_name;
    public string entity_type;

    public string hp = "100/100";


    public bool isOnGround = true;
    public bool isControlled = false;
    public bool entityEnabled = true;

    public float npcHeight = 0.07f;
    protected Camera playerCamera = null;
    

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }

        set
        {
            if(gameObject != null)
            {
                transform.position = value;
            }
        }
    }

    public Vector3 EulerAngles
    {
        get
        {
            return transform.eulerAngles;
        }

        set
        {
            if(gameObject != null)
            {
                transform.eulerAngles = value;
            }
        }
    }
    public Quaternion rotation
    {
        get
        {
            return Quaternion.Euler(EulerAngles);
        }

        set
        {
            EulerAngles = value.eulerAngles;
        }
    }

    public Vector3 Scale
    {
        get
        {
            return _scale;
        }

        set
        {
            _scale = value;
        }
    }

    public uint SpaceID
    {
        get
        {
            return _spaceID;
        }

        set
        {
            _spaceID = value;
        }
    }

    public float Speed
    {
        get
        {
            return _speed;
        }

        set
        {
            _speed = value;
        }
    }

    public float CruiseSpeed
    {
        get
        {
            return cruiseSpeed;
        }

        set
        {
            cruiseSpeed = value;
        }
    }

    public void entityEnable()
    {
        entityEnabled = true;
    }

    public void entityDisable()
    {
        entityEnabled = false;
    }

    public virtual void OnJump()
    {

    }
}
