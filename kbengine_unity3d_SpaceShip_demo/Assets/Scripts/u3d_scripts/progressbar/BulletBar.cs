using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBar : Progressbar
{
    public override void Progressbar_WeaponBronEvent(WeaponAttri weapon)
    {
        if (weapon.BarNode == "bullet_cd" && _weapAttri == null)
        {
            AttachToWeap(weapon);
            SetProgressBar();
        }
    }
}
