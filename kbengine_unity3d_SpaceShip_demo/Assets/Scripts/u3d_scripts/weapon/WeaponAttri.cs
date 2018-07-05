using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttri {

    public UInt32 nID; //武器ID

    public string Name;

    private float _duration = 0.0f; //武器冷却CD
   
    public float CooldownTime = 1.0f; // 正常情况这里读取配置文件

    public float AttackDistance = 50.0f;

    private float _speed;//初始速度

    private float _damage;//伤害值

    private float _explodeTime;

    public string BarNode;

    public float Duration
    {
        get
        {
            return _duration;
        }

        set
        {
            _duration = Mathf.Max(0f,value);
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
            _speed = Mathf.Max(0.0f,value);
        }
    }

    public float Damage
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

    public float ExplodeTime
    {
        get
        {
            return _explodeTime;
        }

        set
        {
            _explodeTime = Mathf.Max(0.0f, value);
        }
    }
}
