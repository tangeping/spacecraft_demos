using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using System;
using CONF;

public class FireWeapon : MonoBehaviour
{
    //-------------飛船------------------//
    ShipBase myship;

    public Dictionary<UInt32, List<FireCondition>> _FireConditions = new Dictionary<UInt32, List<FireCondition>>();
    public Dictionary<UInt32, WeaponAttri> _Weapons = new Dictionary<UInt32, WeaponAttri>();
    public Dictionary<string, List<UInt32> >bullet_list = new Dictionary<string, List<UInt32>>();

    //---------------子弹-------------------//
    [HideInInspector] GameObject weapon_parent = null;


    void LoadBulletCSV()
    {
        Dictionary<UInt32, object> bullet_list = CSVHelper.Instance.GetTable("conf_bullet");

        foreach(var items in bullet_list)
        {
            conf_bullet bullet_conf = (conf_bullet)items.Value;
            WeaponAttri _bullet = new WeaponAttri();

            _bullet.nID = bullet_conf.NID;   //子弹ID
            _bullet.Name = bullet_conf.Name; //名称
            _bullet.CooldownTime = bullet_conf.CooldownTime;   //冷却时间
            _bullet.AttackDistance = bullet_conf.AttackDistance; //攻击距离
            _bullet.Speed = bullet_conf.Speed;           //初始飞行速度
            _bullet.Damage = bullet_conf.Damage;          //攻击伤害
            _bullet.ExplodeTime = bullet_conf.ExplodeTime;  //爆炸时间
            _bullet.BarNode = bullet_conf.BarNode; //cd bar 挂在节点

            _Weapons[_bullet.nID] = _bullet;

            if (!_FireConditions.ContainsKey(_bullet.nID))
            {
                _FireConditions[_bullet.nID] = new List<FireCondition>();
                _FireConditions[_bullet.nID].Add(new IsCooldown());
            }
        }
    }


    private void Awake()
    {
        myship = GetComponent<ShipBase>();
    }
    void Start()
    {
        LoadBulletCSV();
    }

    public void CastSpell(UInt32 nID)
    {
        Unity.Logout.Log("FireWeapon::CastSpell:nID="+ nID);

        if (gameObject == null)
        {
            return;
        }

        if ( _FireConditions.Count <= 0)
        {
            return;
        }

        foreach (var condition in _FireConditions[nID])
        {
            if (!condition.CanFire(_Weapons[nID]))
            {
                return;
            }
        }
        _Weapons[nID].Duration = _Weapons[nID].CooldownTime;

        myship.PlaySparksEffect();

        weapon_parent = Trans.FindObj( gameObject,"weapon_born_" + nID);
        if (weapon_parent == null)
        {
            Debug.LogError("not found " + "weapon_born_" + nID);
        }

        string logStr = "FireWeapon::CastSpell: " +
            "position:" + weapon_parent.transform.position +
            ",eulerAngles:" + weapon_parent.transform.eulerAngles +
            ",forward:" + weapon_parent.transform.forward;

        Debug.Log(logStr);
        Unity.Logout.Log(logStr);


        KBEngine.Event.fireIn("useWeapon",
            weapon_parent.transform.position,
            weapon_parent.transform.eulerAngles,
            weapon_parent.transform.forward, nID);


        GameObject BarObj = Trans.FindObj(Camera.main.gameObject, _Weapons[nID].BarNode);
        if(BarObj != null)
        {
            Progressbar BarScript = BarObj.GetComponent<Progressbar>();
            BarScript.Progressbar_WeaponBronEvent(_Weapons[nID]);
        }
    }

}
