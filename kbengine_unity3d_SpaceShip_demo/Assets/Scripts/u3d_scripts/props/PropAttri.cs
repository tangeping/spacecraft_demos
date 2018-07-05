using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAttri {

    // Use this for initialization
    private int _id;
    private int _damage;
    private int _energy;
    private float _duration = 0.1f;
    private string _loadfile;
    private Vector3 _position;
    private Vector3 _direction;


    public int Damage
    {
        get
        {
            return _damage;
        }

        set
        {
            _damage = value;
        }
    }

    public int Energy
    {
        get
        {
            return _energy;
        }

        set
        {
            _energy = value;
        }
    }

    public int Id
    {
        get
        {
            return _id;
        }

        set
        {
            _id = value;
        }
    }

    public float Duration
    {
        get
        {
            return _duration;
        }

        set
        {
            _duration = value;
        }
    }

    public string Loadfile
    {
        get
        {
            return _loadfile;
        }

        set
        {
            _loadfile = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return _position;
        }

        set
        {
            _position = value;
        }
    }

    public Vector3 Direction
    {
        get
        {
            return _direction;
        }

        set
        {
            _direction = value;
        }
    }
}
