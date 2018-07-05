using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{

    // Use this for initialization
    protected Image _image = null;
    protected WeaponAttri _weapAttri = null;

    void Start()
    {

        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError("BulletBar _image not be found.");
        }

        _image.fillAmount = 0;

        StartCoroutine(OnFlushCD());
 //       FireWeapon.WeaponBronEvent += Progressbar_WeaponBronEvent;
    }

    public virtual void Progressbar_WeaponBronEvent(WeaponAttri weapon)
    {

    }

    protected void AttachToWeap(WeaponAttri weapon)
    {
        _weapAttri = weapon;
    }

    protected void RemoveFromWeap()
    {
        _weapAttri = null;
    }

    protected void SetProgressBar()
    {
        if (_weapAttri != null)
        {
            _image.fillAmount = Mathf.Abs(_weapAttri.Duration / _weapAttri.CooldownTime) * 1;

 //           Debug.Log(name + "::SetProgressBar Duration : " + _weapAttri.Duration);
        }
        
    }
    
    IEnumerator OnFlushCD()
    {
        while (true)
        {
            if (_weapAttri != null && _weapAttri.Duration != 0)
            {
                _weapAttri.Duration -= Time.deltaTime;

                SetProgressBar();

                if (_weapAttri.Duration == 0)
                {
                    RemoveFromWeap();
                    //已经冷却好了，通知其他订阅者
                }
            }
            yield return null;
        }
    }

}
